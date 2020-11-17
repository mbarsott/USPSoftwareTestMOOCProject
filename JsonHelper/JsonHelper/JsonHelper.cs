using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Mbarsott.JsonHelper
{
    public static class JsonHelper
    {
        public static Dictionary<string, int> Tracker = new Dictionary<string, int>();
        public static void Track(string id)
        {
            Tracker[id] += 1;
        }

        public static IEnumerable<JToken> FindNodesByPropertyAndValue(
            JToken node,
            string propertyName,
            string propertyValue)
        {
            Track("A");
            var result = new HashSet<JToken>();
            if (node == null || string.IsNullOrEmpty(propertyName))
            {
                Track("B");
                return result;
            }
            Track("C");
            if (Match(node, propertyName, propertyValue))
            {
                Track("D");
                result.Add(node);
            }
            if (node.Type == JTokenType.Object ||
                node.Type == JTokenType.Array ||
                node.Type == JTokenType.Property)
            {
                Track("E");
                foreach (var child in node.Children())
                {
                    Track("F");
                    if (child.Type == JTokenType.Object ||
                        child.Type == JTokenType.Array ||
                        child.Type == JTokenType.Property)
                    {
                        Track("G");
                        result.UnionWith(
                            FindNodesByPropertyAndValue(
                                child,
                                propertyName,
                                propertyValue));
                    }
                }
            }
            Track("H");
            return result;
        }

        public static bool Match(
            JToken node,
            string propertyName,
            string propertyValue)
        {
            Track("I");
            foreach (var child in node.Children())
            {
                Track("J");
                if (child.Type == JTokenType.Property &&
                    propertyName.Equals(((JProperty)child).Name))
                {
                    Track("K");
                    if (string.IsNullOrEmpty(propertyValue))
                    {
                        Track("L");
                        return true;
                    }
                    if (child.HasValues)
                    {
                        Track("M");
                        if (child.First.Type == JTokenType.Array)
                        {
                            Track("N");
                            foreach (JValue value in child.First.Children())
                            {
                                Track("O");
                                if (value.Type == JTokenType.String)
                                {
                                    Track("P");
                                    if (propertyValue.Equals(value))
                                    {
                                        Track("Q");
                                        return true;
                                    }
                                }
                            }
                        }
                        else if (child.First.Type == JTokenType.String)
                        {
                            Track("R");
                            if (propertyValue.Equals(child.First.ToString()))
                            {
                                Track("S");
                                return true;
                            }
                        }
                    }
                }
            }

            Track("T");
            return false;
        }
    }
}
