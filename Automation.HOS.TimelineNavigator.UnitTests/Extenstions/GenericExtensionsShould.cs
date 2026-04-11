using NUnit.Framework;
using Automation.HOS.TimelineNavigator.Extensions;

namespace Automation.HOS.TimelineNavigator.UnitTests.Extenstions;

[TestFixture]
public class GenericExtensionsShould
{
    // ── helper types ──────────────────────────────────────────────────────────

    private class SimplePoco
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    private class PocoWithList
    {
        public List<int> Items { get; set; } = new();
    }

    private class PocoWithComplexList
    {
        public List<SimplePoco> Items { get; set; } = new();
    }

    private class PocoWithNested
    {
        public SimplePoco? Inner { get; set; }
    }

    // Interface property: not IComparable, not IEnumerable, not a class → hits "Cannot compare" branch
    private interface IOpaque { }
    private class PocoWithOpaque
    {
        public IOpaque? Value { get; set; }
    }

    // Struct without IComparable → hits object.Equals path in AreValuesEqual
    private struct NonComparableStruct { public int X; }
    private class PocoWithStruct
    {
        public NonComparableStruct Value { get; set; }
    }

    private enum Color { Red, Green, Blue }

    // ── CanSerialize ──────────────────────────────────────────────────────────

    [Test]
    public void CanSerialize_RoundTrips_PrimitiveValue()
    {
        var result = 42.CanSerialize();
        Assert.That(result, Is.EqualTo(42));
    }

    [Test]
    public void CanSerialize_RoundTrips_StringValue()
    {
        var result = "hello".CanSerialize();
        Assert.That(result, Is.EqualTo("hello"));
    }

    // ── DeepEquals ── both-null / one-null ───────────────────────────────────

    [Test]
    public void DeepEquals_BothNull_ReturnsTrue()
    {
        Assert.That(((object?)null).DeepEquals(null), Is.True);
    }

    [Test]
    public void DeepEquals_OneNull_ReturnsFalse()
    {
        Assert.That(((object?)null).DeepEquals(new SimplePoco()), Is.False);
        Assert.That(new SimplePoco().DeepEquals(null), Is.False);
    }

    // ── DeepEquals ── primitive / directly-comparable properties ─────────────

    [Test]
    public void DeepEquals_SamePrimitiveProperties_ReturnsTrue()
    {
        var a = new SimplePoco { Id = 1, Name = "A" };
        var b = new SimplePoco { Id = 1, Name = "A" };
        Assert.That(a.DeepEquals(b), Is.True);
    }

    [Test]
    public void DeepEquals_DifferentPrimitiveProperty_ReturnsFalse()
    {
        var a = new SimplePoco { Id = 1, Name = "A" };
        var b = new SimplePoco { Id = 2, Name = "A" };
        Assert.That(a.DeepEquals(b), Is.False);
    }

    [Test]
    public void DeepEquals_IgnoreList_SkipsNamedProperty()
    {
        var a = new SimplePoco { Id = 1, Name = "A" };
        var b = new SimplePoco { Id = 1, Name = "DIFFERENT" };
        Assert.That(a.DeepEquals(b, nameof(SimplePoco.Name)), Is.True);
    }

    // ── DeepEquals ── non-IComparable struct → object.Equals path ────────────

    [Test]
    public void DeepEquals_NonComparableStructProperties_Equal_ReturnsTrue()
    {
        var a = new PocoWithStruct { Value = new NonComparableStruct { X = 5 } };
        var b = new PocoWithStruct { Value = new NonComparableStruct { X = 5 } };
        Assert.That(a.DeepEquals(b), Is.True);
    }

    [Test]
    public void DeepEquals_NonComparableStructProperties_Different_ReturnsFalse()
    {
        var a = new PocoWithStruct { Value = new NonComparableStruct { X = 5 } };
        var b = new PocoWithStruct { Value = new NonComparableStruct { X = 9 } };
        Assert.That(a.DeepEquals(b), Is.False);
    }

    // ── DeepEquals ── IEnumerable properties ─────────────────────────────────

    [Test]
    public void DeepEquals_OneCollectionNullOtherNot_ReturnsFalse()
    {
        var a = new PocoWithList { Items = null! };
        var b = new PocoWithList { Items = new List<int> { 1 } };
        Assert.That(a.DeepEquals(b), Is.False);
    }

    [Test]
    public void DeepEquals_CollectionsDifferentCount_ReturnsFalse()
    {
        var a = new PocoWithList { Items = new List<int> { 1 } };
        var b = new PocoWithList { Items = new List<int> { 1, 2 } };
        Assert.That(a.DeepEquals(b), Is.False);
    }

    [Test]
    public void DeepEquals_CollectionsSameItems_ReturnsTrue()
    {
        var a = new PocoWithList { Items = new List<int> { 1, 2, 3 } };
        var b = new PocoWithList { Items = new List<int> { 1, 2, 3 } };
        Assert.That(a.DeepEquals(b), Is.True);
    }

    [Test]
    public void DeepEquals_CollectionsDifferentItems_ReturnsFalse()
    {
        var a = new PocoWithList { Items = new List<int> { 1, 2, 3 } };
        var b = new PocoWithList { Items = new List<int> { 1, 2, 9 } };
        Assert.That(a.DeepEquals(b), Is.False);
    }

    [Test]
    public void DeepEquals_CollectionsOfComplexItems_Equal_ReturnsTrue()
    {
        var a = new PocoWithComplexList { Items = new List<SimplePoco> { new() { Id = 1, Name = "X" } } };
        var b = new PocoWithComplexList { Items = new List<SimplePoco> { new() { Id = 1, Name = "X" } } };
        Assert.That(a.DeepEquals(b), Is.True);
    }

    [Test]
    public void DeepEquals_CollectionsOfComplexItems_Different_ReturnsFalse()
    {
        var a = new PocoWithComplexList { Items = new List<SimplePoco> { new() { Id = 1, Name = "X" } } };
        var b = new PocoWithComplexList { Items = new List<SimplePoco> { new() { Id = 2, Name = "Y" } } };
        Assert.That(a.DeepEquals(b), Is.False);
    }

    // ── DeepEquals ── nested class property ──────────────────────────────────

    [Test]
    public void DeepEquals_NestedClassProperties_Equal_ReturnsTrue()
    {
        var a = new PocoWithNested { Inner = new SimplePoco { Id = 7, Name = "N" } };
        var b = new PocoWithNested { Inner = new SimplePoco { Id = 7, Name = "N" } };
        Assert.That(a.DeepEquals(b), Is.True);
    }

    [Test]
    public void DeepEquals_NestedClassProperties_Different_ReturnsFalse()
    {
        var a = new PocoWithNested { Inner = new SimplePoco { Id = 7, Name = "N" } };
        var b = new PocoWithNested { Inner = new SimplePoco { Id = 8, Name = "N" } };
        Assert.That(a.DeepEquals(b), Is.False);
    }

    // ── DeepEquals ── interface property → "Cannot compare" branch ───────────

    [Test]
    public void DeepEquals_InterfaceProperty_ReturnsFalse()
    {
        // IOpaque is not IComparable, not IEnumerable, not a class → hits the else/Cannot-compare branch
        var a = new PocoWithOpaque();
        var b = new PocoWithOpaque();
        Assert.That(a.DeepEquals(b), Is.False);
    }

    // ── DeepClone ─────────────────────────────────────────────────────────────

    [Test]
    public void DeepClone_Null_ThrowsArgumentNullException()
    {
        SimplePoco? obj = null;
        Assert.Throws<ArgumentNullException>(() => obj.DeepClone());
    }

    [Test]
    public void DeepClone_Primitive_ReturnsEqualValue()
    {
        var result = 99.DeepClone();
        Assert.That(result, Is.EqualTo(99));
    }

    [Test]
    public void DeepClone_Enum_ReturnsEqualValue()
    {
        var result = Color.Green.DeepClone();
        Assert.That(result, Is.EqualTo(Color.Green));
    }

    [Test]
    public void DeepClone_String_ReturnsEqualValue()
    {
        var result = "clone me".DeepClone();
        Assert.That(result, Is.EqualTo("clone me"));
    }

    [Test]
    public void DeepClone_Array_ReturnsDeepCopy()
    {
        var original = new int[] { 1, 2, 3 };
        var clone = original.DeepClone();

        Assert.That(clone, Is.Not.SameAs(original));
        Assert.That(clone, Is.EqualTo(original));
    }

    [Test]
    public void DeepClone_Class_ReturnsDeepCopy()
    {
        var original = new SimplePoco { Id = 10, Name = "orig" };
        var clone = original.DeepClone();

        Assert.That(clone, Is.Not.SameAs(original));
        Assert.That(clone!.Id, Is.EqualTo(10));
        Assert.That(clone.Name, Is.EqualTo("orig"));
    }

    [Test]
    public void DeepClone_Class_MutatingCloneDoesNotAffectOriginal()
    {
        var original = new SimplePoco { Id = 10, Name = "orig" };
        var clone = original.DeepClone()!;
        clone.Id = 99;

        Assert.That(original.Id, Is.EqualTo(10));
    }
}
