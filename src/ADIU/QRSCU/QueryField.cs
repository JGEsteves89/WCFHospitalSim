using System;

namespace QRSCU
{
    public class QueryField : ICloneable
    {
        internal String name; // Display name
        internal uint tag;  // DICOM tag
        internal String val;  // Value: either query or result value

        // Constructor
        internal QueryField(String name, uint tag)
        {
            this.name = name;
            this.tag = tag;
            val = "";
        }

        /// <summary> clone() override
        /// </summary>
        /// <returns> A clone of this object (with the value set to "")
        /// </returns>
        public virtual Object Clone()
        {
            return new QueryField(name, tag);
        }
    }
}
