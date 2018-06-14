using System;

namespace QRSCU
{
    public class ResultFields
    {
        internal String modelName; //   "PATIENT_ROOT", "STUDY_ROOT", "PATIENT_STUDY_ONLY", or "COMPOSITE_ROOT"
        internal String levelName; //   "PATIENT", "STUDY", "SERIES", "IMAGE" or "FRAME"

        internal int model; // Model id
        internal int level; // Level id

        // Required key fields
        internal QueryField[] fields; // The fields used for this model/level

        // All queries must respond with either the retrieve AE Title
        // (if online) or the file set ID and UID (if offline).
        internal String retrieveAEtitle = null;
        internal String fileSetId = null;
        internal String fileSetUid = null;

        // Constructor
        internal ResultFields(QueryFields queryFields)
        {
            // NOTE: We purposely point at the model and level in the
            //       query fields, since that is the only place we may
            //       change these values
            modelName = queryFields.modelName;
            levelName = queryFields.levelName;
            model = queryFields.model;
            level = queryFields.level;

            // We clone all the fields - which sets their values to ""
            int count = queryFields.fields.Length;
            fields = new QueryField[count];

            // Clone the required fields
            try
            {
                for (int i = 0; i < count; i++) fields[i] = (QueryField)queryFields.fields[i].Clone();
            }
            catch (Exception e)
            {
                Util.printError("Unexpected exception", e);
            }
        }

        // Get value for a field identified by its tag
        internal virtual String getValue(uint tag)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].tag == tag)
                {
                    if (fields[i].val == null) return "";
                    return fields[i].val;
                }
            }
            return "";
        }

        // append results from new query fields
        internal virtual void append(QueryFields queryFields)
        {
            int count = fields.Length + queryFields.fields.Length;
            QueryField[] newFlds = new QueryField[count];

            // Copy the original result
            for (int i = 0; i < fields.Length; i++) newFlds[i] = fields[i];
            // Copy the new fields
            try
            {
                for (int i = 0; i < queryFields.fields.Length; i++) newFlds[i + fields.Length] = (QueryField)queryFields.fields[i];
            }
            catch (Exception e)
            {
                Util.printError("Unexpected exception", e);
            }
            fields = newFlds;
            level = queryFields.level;
            levelName = queryFields.levelName;
        }
    }
}
