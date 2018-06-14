using System;
using Mergecom;

namespace QRSCU
{
    public class QueryFields
    {
        void Init()
        {
            flds = new QueryField[]{
                new QueryField("Patient ID         ", MCdicom.PATIENT_ID),
                new QueryField("Patient Name       ", MCdicom.PATIENTS_NAME),
                new QueryField("Study Instance UID ", MCdicom.STUDY_INSTANCE_UID),
                new QueryField("Study Date         ", MCdicom.STUDY_DATE),
                new QueryField("Study Time         ", MCdicom.STUDY_TIME),
                new QueryField("Accession Number   ", MCdicom.ACCESSION_NUMBER),
                new QueryField("Study ID           ", MCdicom.STUDY_ID),
                new QueryField("Series Instance UID", MCdicom.SERIES_INSTANCE_UID),
                new QueryField("Modality           ", MCdicom.MODALITY),
                new QueryField("Series Number      ", MCdicom.SERIES_NUMBER),
                new QueryField("SOP Instance UID   ", MCdicom.SOP_INSTANCE_UID),
                new QueryField("Image Number       ", MCdicom.IMAGE_NUMBER),
                new QueryField("Simple Frame List  ", MCdicom.SIMPLE_FRAME_LIST)};

            fldArray = new QueryField[][][]
                {new QueryField[][]
                      {new QueryField[]{flds[PATIENT_ID_FIELD], flds[PATIENTS_NAME_FIELD]},
                       new QueryField[]{flds[PATIENT_ID_FIELD], flds[STUDY_INSTANCE_UID_FIELD], flds[STUDY_DATE_FIELD], flds[STUDY_TIME_FIELD], flds[ACCESSION_NUMBER_FIELD], flds[STUDY_ID_FIELD]},
                       new QueryField[]{flds[PATIENT_ID_FIELD], flds[STUDY_INSTANCE_UID_FIELD], flds[SERIES_INSTANCE_UID_FIELD], flds[MODALITY_FIELD], flds[SERIES_NUMBER_FIELD]},
                       new QueryField[]{flds[PATIENT_ID_FIELD], flds[STUDY_INSTANCE_UID_FIELD], flds[SERIES_INSTANCE_UID_FIELD], flds[SOP_INSTANCE_UID_FIELD], flds[IMAGE_NUMBER_FIELD]}},
                     new QueryField[][]
                      {new QueryField[]{null},
                       new QueryField[]{flds[PATIENT_ID_FIELD], flds[PATIENTS_NAME_FIELD], flds[STUDY_INSTANCE_UID_FIELD], flds[STUDY_DATE_FIELD], flds[STUDY_TIME_FIELD], flds[ACCESSION_NUMBER_FIELD], flds[STUDY_ID_FIELD]},
                       new QueryField[]{flds[PATIENT_ID_FIELD], flds[STUDY_INSTANCE_UID_FIELD], flds[SERIES_INSTANCE_UID_FIELD], flds[MODALITY_FIELD], flds[SERIES_NUMBER_FIELD]},
                       new QueryField[]{flds[PATIENT_ID_FIELD], flds[STUDY_INSTANCE_UID_FIELD], flds[SERIES_INSTANCE_UID_FIELD], flds[SOP_INSTANCE_UID_FIELD], flds[IMAGE_NUMBER_FIELD]}},
                     new QueryField[][]
                      {new QueryField[]{flds[PATIENT_ID_FIELD], flds[PATIENTS_NAME_FIELD]},
                       new QueryField[]{flds[PATIENT_ID_FIELD], flds[STUDY_INSTANCE_UID_FIELD], flds[STUDY_DATE_FIELD], flds[STUDY_TIME_FIELD], flds[ACCESSION_NUMBER_FIELD], flds[STUDY_ID_FIELD]},
                       new QueryField[]{null},
                       new QueryField[]{null}},
                     new QueryField[][]
                      {new QueryField[]{flds[PATIENT_ID_FIELD], flds[PATIENTS_NAME_FIELD]},
                       new QueryField[]{flds[PATIENT_ID_FIELD], flds[STUDY_INSTANCE_UID_FIELD], flds[STUDY_DATE_FIELD], flds[STUDY_TIME_FIELD], flds[ACCESSION_NUMBER_FIELD], flds[STUDY_ID_FIELD]},
                       new QueryField[]{flds[PATIENT_ID_FIELD], flds[STUDY_INSTANCE_UID_FIELD], flds[SERIES_INSTANCE_UID_FIELD], flds[MODALITY_FIELD], flds[SERIES_NUMBER_FIELD]},
                       new QueryField[]{flds[PATIENT_ID_FIELD], flds[STUDY_INSTANCE_UID_FIELD], flds[SERIES_INSTANCE_UID_FIELD], flds[SOP_INSTANCE_UID_FIELD] },
                       new QueryField[]{flds[FRAME_LIST_FIELD]}}
                };
        }

        virtual internal int Model
        {
            // Change the query model
            set
            {
                this.model = value;               
                if (value == QRSCU.QuerySCU.PATIENT_ROOT_MODEL) modelName = "PATIENT_ROOT";
                else if (value == QRSCU.QuerySCU.STUDY_ROOT_MODEL) modelName = "STUDY_ROOT";
                else if (value == QRSCU.QuerySCU.PATIENT_STUDY_ONLY_MODEL) modelName = "PATIENT_STUDY_ONLY";
                else if (value == QRSCU.QuerySCU.COMPOSITE_MODEL) modelName = "COMPOSITE_ROOT";

                fields = fldArray[value][level];
            }
        }

        virtual internal int Level
        {
            // Change the query level
            set
            {
                this.level = value;
                if (value == QRSCU.QuerySCU.PATIENT_LEVEL) levelName = "PATIENT";
                else if (value == QRSCU.QuerySCU.STUDY_LEVEL) levelName = "STUDY";
                else if (value == QRSCU.QuerySCU.SERIES_LEVEL) levelName = "SERIES";
                else if (value == QRSCU.QuerySCU.IMAGE_LEVEL) levelName = "IMAGE";
                else if (value == QRSCU.QuerySCU.FRAME_LEVEL) levelName = "FRAME";

                fields = fldArray[model][value];
            }
        }

        internal String modelName; //   "PATIENT_ROOT", "STUDY_ROOT", "PATIENT_STUDY_ONLY", or "COMPOSITE_ROOT"
        internal String levelName; //   "PATIENT", "STUDY", "SERIES", "IMAGE" or "FRAME"

        internal int model; // Query/Retrieve Model id
        internal int level; // Query/Retrieve Level id
        internal int startLevel; // Query started at this level

        // Fields used in the current query
        // (Array elements are all references to QueryFields in the flds array)
        internal QueryField[] fields;

        // Constants for the indexes of the flds array
        internal const int PATIENT_ID_FIELD = 0;
        internal const int PATIENTS_NAME_FIELD = 1;

        internal const int STUDY_INSTANCE_UID_FIELD = 2;
        internal const int STUDY_DATE_FIELD = 3;
        internal const int STUDY_TIME_FIELD = 4;
        internal const int ACCESSION_NUMBER_FIELD = 5;
        internal const int STUDY_ID_FIELD = 6;

        internal const int SERIES_INSTANCE_UID_FIELD = 7;
        internal const int MODALITY_FIELD = 8;
        internal const int SERIES_NUMBER_FIELD = 9;

        internal const int SOP_INSTANCE_UID_FIELD = 10;
        internal const int IMAGE_NUMBER_FIELD = 11;

        internal const int FRAME_LIST_FIELD = 12;

        // We support Unique and Required Fields at each DICOM level
        internal QueryField[] flds;

        // Index lists for the different model/level combinations
        // Array key is (model, level, fieldIndex)
        internal QueryField[][][] fldArray;

        // Constructor
        internal QueryFields(int model, int level)
        {
            Init();

            this.model = model;
            this.level = level;
            this.startLevel = level;

            Model = model;
            Level = level;
        }

        // Change the query level to the top level for the model
        internal virtual void setTopLevel()
        {
            if (model == QRSCU.QuerySCU.STUDY_ROOT_MODEL) Level = QRSCU.QuerySCU.STUDY_LEVEL;
            else Level = QRSCU.QuerySCU.PATIENT_LEVEL;
        }

        //  Update query keys from a query result
        internal virtual void updateKeys(ResultFields result)
        {
            String val = result.getValue(MCdicom.PATIENT_ID);
            if (val != null) flds[PATIENT_ID_FIELD].val = val;

            val = result.getValue(MCdicom.PATIENTS_NAME);
            if (val != null) flds[PATIENTS_NAME_FIELD].val = val;

            val = result.getValue(MCdicom.STUDY_INSTANCE_UID);
            if (val != null) flds[STUDY_INSTANCE_UID_FIELD].val = val;

            val = result.getValue(MCdicom.STUDY_DATE);
            if (val != null) flds[STUDY_DATE_FIELD].val = val;

            val = result.getValue(MCdicom.STUDY_TIME);
            if (val != null) flds[STUDY_TIME_FIELD].val = val;

            val = result.getValue(MCdicom.ACCESSION_NUMBER);
            if (val != null) flds[ACCESSION_NUMBER_FIELD].val = val;

            val = result.getValue(MCdicom.STUDY_ID);
            if (val != null) flds[STUDY_ID_FIELD].val = val;

            val = result.getValue(MCdicom.SERIES_INSTANCE_UID);
            if (val != null) flds[SERIES_INSTANCE_UID_FIELD].val = val;

            val = result.getValue(MCdicom.MODALITY);
            if (val != null) flds[MODALITY_FIELD].val = val;

            val = result.getValue(MCdicom.SERIES_NUMBER);
            if (val != null) flds[SERIES_NUMBER_FIELD].val = val;

            val = result.getValue(MCdicom.SOP_INSTANCE_UID);
            if (val != null) flds[SOP_INSTANCE_UID_FIELD].val = val;

            val = result.getValue(MCdicom.IMAGE_NUMBER);
            if (val != null) flds[IMAGE_NUMBER_FIELD].val = val;
        }

        // Advance one level down
        //
        // Using the unique keys from the last result
        // set up to continue the query one level down.
        // Returns the possibly-changed level id.
        internal virtual int bumpLevel(ResultFields result)
        {
            // Return if already at the lowest level
            if ((model == QRSCU.QuerySCU.PATIENT_STUDY_ONLY_MODEL && level == QRSCU.QuerySCU.STUDY_LEVEL) ||
                level == QRSCU.QuerySCU.IMAGE_LEVEL)
                return level;

            // Update query keys from this result
            updateKeys(result);

            // Bump to next level
            Level = ++level;
            return level;
        }
    }
}
