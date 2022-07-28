using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestContextMapperExtensionTests.TestObjects
{
    public class NestedObjectBase
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
        public List<string> ListOfStringProperty { get; set; }
        public Dictionary<string, string> DictionaryOfStringStringProperty { get; set; }
        public NestedObjectLayerTwo Nested1Object1 { get; set; }
        public NestedObjectLayerTwo Nested1Object2 { get; set; }
    }

    public class NestedObjectLayerTwo
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
        public List<string> ListOfStringProperty { get; set; }
        public Dictionary<string, string> DictionaryOfStringStringProperty { get; set; }
        public NestedObjectLayerThree Nested2Object1 { get; set; }
        public NestedObjectLayerThree Nested2Object2 { get; set; }
    }

    public class NestedObjectLayerThree
    {
        public string FirstProperty { get; set; }
        public string SecondProperty { get; set; }
    }
}
