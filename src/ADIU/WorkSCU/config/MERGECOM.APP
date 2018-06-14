#============================================================================
#                      MERGECOM-3 Application Configurations
#============================================================================
#
#                   ==== MergeCOM-3 Application Profiles ===
#
#      The location of this file is provided in the MERGECOM_3_APPLICATIONS
#          parameter of the [MergeCOM3] section of the MERGE.INI file
# .
#============================================================================
#
#  Contains the following sections:
#    [<remote_application_title>]- Section describing a remote DICOM application
#                                  <remote_application_title> names must be
#                                  1 to 16 bytes in length with no embedded 
#                                  spaces.  This section is only used by 
#                                  SCU applications to configure systems they 
#                                  will connect with.
#                                 
#    [<service_list_name>]      - List[s] of DICOM services (referenced by
#                                 entries in the [<remote_application_title>] 
#                                 sections.  These section names are also used
#                                 by the MC_Wait_For_Association function to
#                                 specify an SCP's services supported.
#                                 <service_list_name> names must be
#                                 1 to 33 bytes in length with no embedded 
#                                 spaces.  
#                                 
#    [<syntax_list_name>]       - List[s] of DICOM transfer syntaxes (referenced 
#                                 by optional entries in the [<service_list_name>]
#                                 sections. 
#                                 <syntax_list_name> names must be
#                                 1 to 33 bytes in length with no embedded 
#                                 spaces.
#


# Each [<remote_application_title>] section is of this format:
#       PORT_NUMBER     = <port>          is the TCP/IP port on which the 
#                                         remote DICOM system listens for 
#                                         connections.  The commonly used port 
#                                         number is 104. This default value may
#                                         be overriden by the 
#                                         MC_Open_Association() function call.
#       HOST_NAME       = <TCP/IP name>   is the name of the remote host as it
#                                         is known to your TCP/IP system.  This
#                                         default value may be overriden by 
#                                         the MC_Open_Association() function 
#                                         call.
#                                         <TCP/IP name> must be 1 to 19 bytes 
#                                         in length with no embedded spaces.
#       SERVICE_LIST    = <list name>     is the name of another section in 
#                                         this file which provides a list of 
#                                         services local applications will 
#                                         negotiate for when attempting to
#                                         establish an association.  This is a
#                                         default list; another list may be 
#                                         specified in the 
#                                         MC_Open_Association() call.
#                                         <list name> names must be 1 to 33 
#                                         bytes in length with no embedded 
#                                         spaces.

#
# NOTE: that if the storage commitment SCU and SCP are running on the same machine, 
# a new port number must be placed so both applications can listen on different ports.
#

[MERGE_STORE_SCP]
    PORT_NUMBER     = 104        # port 104 is the standard DICOM port and is privileged
    HOST_NAME       = localhost  # Put the remote machine name here
    SERVICE_LIST    = Storage_SCU_Service_List

[MERGE_STORE_SCU]
    PORT_NUMBER     = 1115       # port 104 is the standard DICOM port and is privileged
    HOST_NAME       = localhost  # Put the remote machine name here
    SERVICE_LIST    = Storage_Commit_SCP_Service_List

[MERGE_MEDIA_FSU]
    PORT_NUMBER     = 104
    HOST_NAME       = localhost
    SERVICE_LIST    = Storage_SCP_Service_List 
#
# This example has a different port to prevent conflicts when
# testing both applications on the same machine.  
# Both the Q/R SCU and SCP samples SCP samples listen for associations.
#
[MERGE_QR_SCP]
    PORT_NUMBER     = 104
    HOST_NAME       = localhost
    SERVICE_LIST    = Query_SCP_Service_List
    
[MERGE_QR_SCU]
    PORT_NUMBER     = 1115
    HOST_NAME       = localhost
    SERVICE_LIST    = Storage_SCP_Service_List    

[MERGE_QR_GET_SCP]
    PORT_NUMBER     = 104
    HOST_NAME       = localhost
    SERVICE_LIST    = GET_SCP_Service_List

[MERGE_QR_GET_SCU]
    PORT_NUMBER     = 1115
    HOST_NAME       = localhost
    SERVICE_LIST    = GET_SCU_Service_List    

[MERGE_PRINT_SCP]
    PORT_NUMBER     = 104
    HOST_NAME       = localhost
    SERVICE_LIST    = Print_Service_List

[MERGE_WORK_SCP]
    PORT_NUMBER     = 104
    HOST_NAME       = localhost
    SERVICE_LIST    = Worklist_Service_List

[MERGE_UPS_SCP]
    PORT_NUMBER     = 104
    HOST_NAME       = localhost
    SERVICE_LIST    = UPS_SERVICE_LIST

[MERGE_UPS_SCU]
    PORT_NUMBER     = 1115
    HOST_NAME       = localhost
    SERVICE_LIST    = UPS_SERVICE_LIST    

[MERGE_UPS_EVN]
    PORT_NUMBER     = 105
    HOST_NAME       = localhost
    SERVICE_LIST    = UPS_SERVICE_LIST

[UPS_TEST_SCU]
    PORT_NUMBER     = 104        
    HOST_NAME       = localhost
    SERVICE_LIST    = UPS_SERVICE_LIST

[UPS_TEST_SCP]
    PORT_NUMBER     = 104        
    HOST_NAME       = localhost
    SERVICE_LIST    = UPS_SERVICE_LIST

[TEST_AE_SCP]
    PORT_NUMBER     = 104
    HOST_NAME       = localhost
    SERVICE_LIST    = AETitles_Service_List

[TEST_AE_SCU1]
    PORT_NUMBER     = 104
    HOST_NAME       = localhost
    SERVICE_LIST    = AETitles_Service_List

[TEST_AE_SCU2]
    PORT_NUMBER     = 104
    HOST_NAME       = localhost
    SERVICE_LIST    = AETitles_Service_List

[STREAM_SCP]
    PORT_NUMBER     = 104        # port 104 is the standard DICOM port and is privileged
    HOST_NAME       = localhost  # Put the remote machine name here
    SERVICE_LIST    = Stream_SCU_Service_List

[STREAM_SCU]
    PORT_NUMBER     = 104        # port 104 is the standard DICOM port and is privileged
    HOST_NAME       = myhost     # Put the remote machine name here
    SERVICE_LIST    = Stream_SCP_Service_List

# Each [<service_list_name>] section must contain a SERVICES_SUPPORTED
#       parameter to specify the number of services in the list.  Optionally,
#       if an application supports DICOM asynchronous communications, the 
#       maximum operations invoked and performed by the application can be
#       specified in the service list.  MAX_OPERATIONS_INVOKED specifies
#       the maximum number of requests messages an application will send
#       before processing a response message.  MAX_OPERATIONS_PERFORMED
#       specifies how many requests messages an application can recieve
#       before sending a response message.  Setting a value of '0' means
#       unlimited requests can be invoked or performed.  Leaving these 
#       options out of the service list will cause this not to be negotiated
#       and synchronous communication will be used over the association.
#
#       MAX_OPERATIONS_INVOKED = 10
#       MAX_OPERATIONS_PERFORMED = 10
#
#       Finally, a service list has five parameters for each service, of 
#       the following format:
#
#       SERVICE_n = <service name>
#       SYNTAX_LIST_n = <syntax_list_name>  (this parameter is optional)
#       ROLE_n = <role type>
#       EXT_NEG_INFO_n = <neg info>
#       REL_GENERAL_n = <service list name>
#       SERVICE_CLASS_n = <service class UID>
#
#       where:
#               SERVICE_n       the character string "SERVICE_n", with n 
#                               replaced by the number of the service being 
#                               named.
#               ROLE_n          the character string "ROLE_n", with n replaced
#                               by the number of the role being set.  The only
#                               values which may be set are SCU, SCP, or BOTH.
#                               This section is optional.  Not specifying a
#                               role will default to a requestor = SCU, and
#                               acceptor = SCP.
#               SYNTAX_LIST_n   the character string "SYNTAX_LIST_n", with n 
#                               replaced by the number of the service for 
#                               which a list of transfer syntaxes to support
#                               is specified.
#               EXT_NEG_INFO_n  the character string "EXT_NEG_INFO_n", with n 
#                               replaced by the number of the service for 
#                               which extended negotiation is specified.  Note
#                               that this extended negotation information is 
#                               only used by the SCU.
#               SERVICE_CLASS_n the character string "SERVICE_CLASS_N", with n 
#                               replaced by the number of the service for 
#                               which the service class UID is specified.  This
#                               item must be specified if the REL_GENERAL_n
#                               listing is specified and is specified as 
#                               defined in DICOM supplement 90.
#               REL_GENERAL_n   the character string "REL_GENERAL_n", with n 
#                               replaced by the number of the service for 
#                               which related general SOP Classes are specified
#                               as defined in DICOM supplement 90.  This 
#                               setting points to another service list that
#                               contains generalized SOP Classes for this
#                               specific SOP Class.  These generalized SOP 
#                               classes are included in an association request
#                               PDU when specified.
#               <service name>  is the name of one of the services supported by
#                               your MergeCOM-3 system.  It must match one of 
#                               the service names described in the MergeCOM-3 
#                               Services Profile (named by the 
#                               MERGECOM_3_SERVICES parameter in the merge.ini 
#                               file).
#               <syntax_list_name>  is the name of a section that contains a 
#                               list of transfer syntaxes to support.  This
#                               parameter is optional.
#               <neg info>      is a string of hex characters signaling the 
#                               DICOM extended negotiation info to be used by
#                               an SCU when proposing an association.  The 
#                               string would be of the format 0x01 0x00 0x01, 
#                               etc.
#               <service list name> The name of a service list that contains
#                               the related general SOP Classes to this service.
#               <service class uid>  The service class UID of this service.
#                               Currently, the only defined service class UID
#                               is for the Storage Service.  It's UID is:
#                               1.2.840.10008.4.2       
#
# If a transfer syntax list is not specified, the tool kit will use the three
# non-encapsulated transfer syntaxes defined in the mergecom.pro configuration
# file (implicit VR little endian, explicit VR little endian and explicit VR big
# endian).

#
# Service list used by the Storage and Storage Commitment SCP application.
# This is a seperate service list because the SCU does not want to include
# the storage commitment push service class its association request.
#
[Storage_SCP_Service_List]
    SERVICES_SUPPORTED      = 82       # Number of Services in list
    SERVICE_1               = STORAGE_COMMITMENT_PUSH
    SERVICE_2               = CHEST_CAD_SR
    SERVICE_3               = ENHANCED_CT_IMAGE
    SERVICE_4               = ENHANCED_MR_IMAGE
    SERVICE_5               = KEY_OBJECT_SELECTION_DOC
    SERVICE_6               = MAMMOGRAPHY_CAD_SR
    SERVICE_7               = MR_SPECTROSCOPY
    SERVICE_8               = PROCEDURE_LOG
    SERVICE_9               = SC_MULTIFRAME_GRAYSCALE_BYTE    
    SERVICE_10              = SC_MULTIFRAME_GRAYSCALE_WORD
    SERVICE_11              = SC_MULTIFRAME_SINGLE_BIT
    SERVICE_12              = SC_MULTIFRAME_TRUE_COLOR
    SERVICE_13              = SPATIAL_FIDUCIALS
    SERVICE_14              = SPATIAL_REGISTRATION
    SERVICE_15              = STANDARD_BASIC_TEXT_SR
    SERVICE_16              = STANDARD_COMPREHENSIVE_SR
    SERVICE_17              = STANDARD_CR
    SERVICE_18              = STANDARD_CT
    SYNTAX_LIST_18          = Uncompressed_Syntax_List    # This is provided as an example and can be removed
    SERVICE_19              = STANDARD_CURVE
    SERVICE_20              = STANDARD_DX_PRESENT
    SERVICE_21              = STANDARD_DX_PROCESS
    SERVICE_22              = STANDARD_ENHANCED_SR
    SERVICE_23              = STANDARD_GRAYSCALE_SOFTCOPY_PS
    SERVICE_24              = STANDARD_HARDCOPY_COLOR
    SERVICE_25              = STANDARD_HARDCOPY_GRAYSCALE
    SERVICE_26              = STANDARD_IO_PRESENT
    SERVICE_27              = STANDARD_IO_PROCESS
    SERVICE_28              = STANDARD_MG_PRESENT
    SERVICE_29              = STANDARD_MG_PROCESS
    SERVICE_30              = STANDARD_MODALITY_LUT
    SERVICE_31              = STANDARD_MR
    SERVICE_32              = STANDARD_NM
    SERVICE_33              = STANDARD_NM_RETIRED
    SERVICE_34              = STANDARD_OPHTHALMIC_16_BIT
    SERVICE_35              = STANDARD_OPHTHALMIC_8_BIT
    SERVICE_36              = STANDARD_OVERLAY
    SERVICE_37              = STANDARD_PET
    SERVICE_38              = STANDARD_PET_CURVE
    SERVICE_39              = STANDARD_PRINT_STORAGE
    SERVICE_40              = STANDARD_RT_BEAMS_TREAT
    SERVICE_41              = STANDARD_RT_BRACHY_TREAT
    SERVICE_42              = STANDARD_RT_DOSE
    SERVICE_43              = STANDARD_RT_IMAGE
    SERVICE_44              = STANDARD_RT_PLAN
    SERVICE_45              = STANDARD_RT_STRUCTURE_SET
    SERVICE_46              = STANDARD_RT_TREAT_SUM
    SERVICE_47              = STANDARD_SEC_CAPTURE
    SERVICE_48              = STANDARD_US
    SERVICE_49              = STANDARD_US_MF
    SERVICE_50              = STANDARD_US_MF_RETIRED
    SERVICE_51              = STANDARD_US_RETIRED
    SERVICE_52              = STANDARD_VIDEO_ENDOSCOPIC 
    SERVICE_53              = STANDARD_VIDEO_MICROSCOPIC
    SERVICE_54              = STANDARD_VIDEO_PHOTOGRAPHIC
    SERVICE_55              = STANDARD_VL_ENDOSCOPIC
    SERVICE_56              = STANDARD_VL_MICROSCOPIC
    SERVICE_57              = STANDARD_VL_PHOTOGRAPHIC
    SERVICE_58              = STANDARD_VL_SLIDE_MICROSCOPIC
    SERVICE_59              = STANDARD_VOI_LUT
    SERVICE_60              = STANDARD_WAVEFORM_12_LEAD_ECG
    SERVICE_61              = STANDARD_WAVEFORM_AMBULATORY_ECG
    SERVICE_62              = STANDARD_WAVEFORM_BASIC_VOICE_AU
    SERVICE_63              = STANDARD_WAVEFORM_CARDIAC_EP
    SERVICE_64              = STANDARD_WAVEFORM_GENERAL_ECG
    SERVICE_65              = STANDARD_WAVEFORM_HEMODYNAMIC
    SERVICE_66              = STANDARD_XRAY_ANGIO
    SERVICE_67              = STANDARD_XRAY_ANGIO_BIPLANE
    SERVICE_68              = STANDARD_XRAY_RF
    SERVICE_69              = STEREOMETRIC_RELATIONSHIP
    SERVICE_70              = ENHANCED_XA_IMAGE
    SERVICE_71              = ENHANCED_XRF_IMAGE
    SERVICE_72              = RAW_DATA
    SERVICE_73              = REAL_WORLD_VALUE_MAPPING
    SERVICE_74              = STANDARD_BLENDING_SOFTCOPY_PS
    SERVICE_75              = STANDARD_COLOR_SOFTCOPY_PS
    SERVICE_76              = STANDARD_ENCAPSULATED_PDF
    SERVICE_77              = STANDARD_PSEUDOCOLOR_SOFTCOPY_PS
    SERVICE_78              = STANDARD_RT_ION_BEAMS_TREAT
    SERVICE_79              = STANDARD_RT_ION_PLAN
    SERVICE_80              = XRAY_RADIATION_DOSE_SR
    SERVICE_81              = DEFORMABLE_SPATIAL_REGISTRATION
    SERVICE_82              = SEGMENTATION


# Storage SCU service list.  This includes all of the standard image types.
[Storage_SCU_Service_List]
    SERVICES_SUPPORTED      = 81       # Number of Services in list 
    MAX_OPERATIONS_INVOKED  = 1
    MAX_OPERATIONS_PERFORMED = 1
    SERVICE_1               = XRAY_RADIATION_DOSE_SR
    SERVICE_2               = CHEST_CAD_SR
    SERVICE_3               = ENHANCED_CT_IMAGE
    SERVICE_4               = ENHANCED_MR_IMAGE
    SERVICE_5               = KEY_OBJECT_SELECTION_DOC
    SERVICE_6               = MAMMOGRAPHY_CAD_SR
    SERVICE_7               = MR_SPECTROSCOPY
    SERVICE_8               = PROCEDURE_LOG
    SERVICE_9               = SC_MULTIFRAME_GRAYSCALE_BYTE    
    SERVICE_10              = SC_MULTIFRAME_GRAYSCALE_WORD
    SERVICE_11              = SC_MULTIFRAME_SINGLE_BIT
    SERVICE_12              = SC_MULTIFRAME_TRUE_COLOR
    SERVICE_13              = SPATIAL_FIDUCIALS
    SERVICE_14              = SPATIAL_REGISTRATION
    SERVICE_15              = STANDARD_BASIC_TEXT_SR
    SERVICE_16              = STANDARD_COMPREHENSIVE_SR
    SERVICE_17              = STANDARD_CR
    SERVICE_18              = STANDARD_CT
    SERVICE_19              = STANDARD_CURVE
    SERVICE_20              = STANDARD_DX_PRESENT
    SERVICE_21              = STANDARD_DX_PROCESS
    SERVICE_22              = STANDARD_ENHANCED_SR
    SERVICE_23              = STANDARD_GRAYSCALE_SOFTCOPY_PS
    SERVICE_24              = STANDARD_HARDCOPY_COLOR
    SERVICE_25              = STANDARD_HARDCOPY_GRAYSCALE
    SERVICE_26              = STANDARD_IO_PRESENT
    SERVICE_27              = STANDARD_IO_PROCESS
    SERVICE_28              = STANDARD_MG_PRESENT
    SERVICE_29              = STANDARD_MG_PROCESS
    SERVICE_30              = STANDARD_MODALITY_LUT
    SERVICE_31              = STANDARD_MR
    SERVICE_32              = STANDARD_NM
    SERVICE_33              = STANDARD_NM_RETIRED
    SERVICE_34              = STANDARD_OPHTHALMIC_16_BIT
    SERVICE_35              = STANDARD_OPHTHALMIC_8_BIT
    SERVICE_36              = STANDARD_OVERLAY
    SERVICE_37              = STANDARD_PET
    SERVICE_38              = STANDARD_PET_CURVE
    SERVICE_39              = STANDARD_PRINT_STORAGE
    SERVICE_40              = STANDARD_RT_BEAMS_TREAT
    SERVICE_41              = STANDARD_RT_BRACHY_TREAT
    SERVICE_42              = STANDARD_RT_DOSE
    SERVICE_43              = STANDARD_RT_IMAGE
    SERVICE_44              = STANDARD_RT_PLAN
    SERVICE_45              = STANDARD_RT_STRUCTURE_SET
    SERVICE_46              = STANDARD_RT_TREAT_SUM
    SERVICE_47              = STANDARD_SEC_CAPTURE
    SERVICE_48              = STANDARD_US
    SERVICE_49              = STANDARD_US_MF
    SERVICE_50              = STANDARD_US_MF_RETIRED
    SERVICE_51              = STANDARD_US_RETIRED
    SERVICE_52              = STANDARD_VIDEO_ENDOSCOPIC 
    SERVICE_53              = STANDARD_VIDEO_MICROSCOPIC
    SERVICE_54              = STANDARD_VIDEO_PHOTOGRAPHIC
    SERVICE_55              = STANDARD_VL_ENDOSCOPIC
    SERVICE_56              = STANDARD_VL_MICROSCOPIC
    SERVICE_57              = STANDARD_VL_PHOTOGRAPHIC
    SERVICE_58              = STANDARD_VL_SLIDE_MICROSCOPIC
    SERVICE_59              = STANDARD_VOI_LUT
    SERVICE_60              = STANDARD_WAVEFORM_12_LEAD_ECG
    SERVICE_61              = STANDARD_WAVEFORM_AMBULATORY_ECG
    SERVICE_62              = STANDARD_WAVEFORM_BASIC_VOICE_AU
    SERVICE_63              = STANDARD_WAVEFORM_CARDIAC_EP
    SERVICE_64              = STANDARD_WAVEFORM_GENERAL_ECG
    SERVICE_65              = STANDARD_WAVEFORM_HEMODYNAMIC
    SERVICE_66              = STANDARD_XRAY_ANGIO
    SERVICE_67              = STANDARD_XRAY_ANGIO_BIPLANE
    SERVICE_68              = STANDARD_XRAY_RF
    SERVICE_69              = STEREOMETRIC_RELATIONSHIP
    SERVICE_70              = ENHANCED_XA_IMAGE
    SERVICE_71              = ENHANCED_XRF_IMAGE
    SERVICE_72              = RAW_DATA
    SERVICE_73              = REAL_WORLD_VALUE_MAPPING
    SERVICE_74              = STANDARD_BLENDING_SOFTCOPY_PS
    SERVICE_75              = STANDARD_COLOR_SOFTCOPY_PS
    SERVICE_76              = STANDARD_ENCAPSULATED_PDF
    SERVICE_77              = STANDARD_PSEUDOCOLOR_SOFTCOPY_PS
    SERVICE_78              = STANDARD_RT_ION_BEAMS_TREAT
    SERVICE_79              = STANDARD_RT_ION_PLAN
    SERVICE_80              = DEFORMABLE_SPATIAL_REGISTRATION
    SERVICE_81              = SEGMENTATION

# Stream SCP service list. 
[Stream_SCP_Service_List]
    SERVICES_SUPPORTED      = 8       # Number of Services in list
    SERVICE_1               = STANDARD_MR
    SYNTAX_LIST_1           = Uncompressed_Syntax_List
    SERVICE_2               = STANDARD_MR
    SYNTAX_LIST_2           = 01_Syntax_List
    SERVICE_3               = STANDARD_MR
    SYNTAX_LIST_3           = 02_Syntax_List
    SERVICE_4               = STANDARD_MR
    SYNTAX_LIST_4           = 03_Syntax_List
    SERVICE_5               = STANDARD_MR
    SYNTAX_LIST_5           = 04_Syntax_List
    SERVICE_6               = STANDARD_MR
    SYNTAX_LIST_6           = 05_Syntax_List
    SERVICE_7               = STANDARD_MR
    SYNTAX_LIST_7           = 06_Syntax_List
    SERVICE_8               = STANDARD_MR
    SYNTAX_LIST_8           = 07_Syntax_List
    

# Stream SCU service list. 
[Stream_SCU_Service_List]
    SERVICES_SUPPORTED      = 8     # Number of Services in list
    SERVICE_1               = STANDARD_MR
    SYNTAX_LIST_1           = Uncompressed_Syntax_List
    SERVICE_2               = STANDARD_MR
    SYNTAX_LIST_2           = 01_Syntax_List
    SERVICE_3               = STANDARD_MR
    SYNTAX_LIST_3           = 02_Syntax_List
    SERVICE_4               = STANDARD_MR
    SYNTAX_LIST_4           = 03_Syntax_List
    SERVICE_5               = STANDARD_MR
    SYNTAX_LIST_5           = 04_Syntax_List
    SERVICE_6               = STANDARD_MR
    SYNTAX_LIST_6           = 05_Syntax_List
    SERVICE_7               = STANDARD_MR
    SYNTAX_LIST_7           = 06_Syntax_List
    SERVICE_8               = STANDARD_MR
    SYNTAX_LIST_8           = 07_Syntax_List

#
# The following 2 service lists are provided as an example of how to define a service list 
# that supports compressed transfer syntaxes. They are not used by any sample application. 
# 
# The SCU_Enc_... service list is a typical SCU's service list. It tries to negotiation both an uncompressed 
# and a compressed transfer syntax for each service.
#
# The SCP_Enc_... service list simply defines all of the transfer syntaxes supported for each DICOM service.
#
[SCU_Enc_Storage_Service_List]
    SERVICES_SUPPORTED      = 4       # Number of Services in list    
    SERVICE_1               = STANDARD_MR
    SYNTAX_LIST_1           = Uncompressed_Syntax_List
    SERVICE_2               = STANDARD_MR
    SYNTAX_LIST_2           = Compressed_Syntax_List
    SERVICE_3               = STANDARD_CT
    SYNTAX_LIST_3           = Uncompressed_Syntax_List
    SERVICE_4               = STANDARD_CT
    SYNTAX_LIST_4           = Compressed_Syntax_List
    
[SCP_Enc_Storage_Service_List]
    SERVICES_SUPPORTED      = 2       # Number of Services in list    
    SERVICE_1               = STANDARD_MR
    SYNTAX_LIST_1           = Full_Syntax_List
    SERVICE_2               = STANDARD_CT
    SYNTAX_LIST_2           = Full_Syntax_List

#
# This is used by the storage SCU sample to open an association with 
# the storage SCP.  Note that the standard "Storage_SCU_Service_List" 
# above for the MERGE_STORE_SCP application is overridden with the
# following service list.
#
[Storage_Commit_SCU_Service_List]
    SERVICES_SUPPORTED      = 1       # Number of Services in list    
    SERVICE_1               = STORAGE_COMMITMENT_PUSH
    ROLE_1                  = SCU

#
# This service list is used by the storage SCP sample application to 
# open an assocation up with the storage SCU.  This association is only
# used to send the Storage Commitment N-EVENT-REPORT-RQ message.
#
[Storage_Commit_SCP_Service_List]
    SERVICES_SUPPORTED      = 1       # Number of Services in list    
    SERVICE_1               = STORAGE_COMMITMENT_PUSH
    ROLE_1                  = SCP

[Query_SCP_Service_List]
    SERVICES_SUPPORTED      = 88     # Number of Services in list
    SERVICE_1               = PATIENT_STUDY_ONLY_QR_FIND
    SERVICE_2               = PATIENT_STUDY_ONLY_QR_MOVE
    SERVICE_3               = STUDY_ROOT_QR_FIND
    EXT_NEG_INFO_3          = 0x01
    SERVICE_4               = STUDY_ROOT_QR_MOVE
    SERVICE_5               = PATIENT_ROOT_QR_FIND
    SERVICE_6               = PATIENT_ROOT_QR_MOVE
    SERVICE_7               = XRAY_RADIATION_DOSE_SR
    SERVICE_8               = CHEST_CAD_SR
    SERVICE_9               = ENHANCED_CT_IMAGE
    SERVICE_10              = ENHANCED_MR_IMAGE
    SERVICE_11              = KEY_OBJECT_SELECTION_DOC
    SERVICE_12              = MAMMOGRAPHY_CAD_SR
    SERVICE_13              = MR_SPECTROSCOPY
    SERVICE_14              = PROCEDURE_LOG
    SERVICE_15              = SC_MULTIFRAME_GRAYSCALE_BYTE    
    SERVICE_16              = SC_MULTIFRAME_GRAYSCALE_WORD
    SERVICE_17              = SC_MULTIFRAME_SINGLE_BIT
    SERVICE_18              = SC_MULTIFRAME_TRUE_COLOR
    SERVICE_19              = SPATIAL_FIDUCIALS
    SERVICE_20              = SPATIAL_REGISTRATION
    SERVICE_21              = STANDARD_BASIC_TEXT_SR
    SERVICE_22              = STANDARD_COMPREHENSIVE_SR
    SERVICE_23              = STANDARD_CR
    SERVICE_24              = STANDARD_CT
    SERVICE_25              = STANDARD_CURVE
    SERVICE_26              = STANDARD_DX_PRESENT
    SERVICE_27              = STANDARD_DX_PROCESS
    SERVICE_28              = STANDARD_ENHANCED_SR
    SERVICE_29              = STANDARD_GRAYSCALE_SOFTCOPY_PS
    SERVICE_30              = STANDARD_HARDCOPY_COLOR
    SERVICE_31              = STANDARD_HARDCOPY_GRAYSCALE
    SERVICE_32              = STANDARD_IO_PRESENT
    SERVICE_33              = STANDARD_IO_PROCESS
    SERVICE_34              = STANDARD_MG_PRESENT
    SERVICE_35              = STANDARD_MG_PROCESS
    SERVICE_36              = STANDARD_MODALITY_LUT
    SERVICE_37              = STANDARD_MR
    SERVICE_38              = STANDARD_NM
    SERVICE_39              = STANDARD_NM_RETIRED
    SERVICE_40              = STANDARD_OPHTHALMIC_16_BIT
    SERVICE_41              = STANDARD_OPHTHALMIC_8_BIT
    SERVICE_42              = STANDARD_OVERLAY
    SERVICE_43              = STANDARD_PET
    SERVICE_44              = STANDARD_PET_CURVE
    SERVICE_45              = STANDARD_PRINT_STORAGE
    SERVICE_46              = STANDARD_RT_BEAMS_TREAT
    SERVICE_47              = STANDARD_RT_BRACHY_TREAT
    SERVICE_48              = STANDARD_RT_DOSE
    SERVICE_49              = STANDARD_RT_IMAGE
    SERVICE_50              = STANDARD_RT_PLAN
    SERVICE_51              = STANDARD_RT_STRUCTURE_SET
    SERVICE_52              = STANDARD_RT_TREAT_SUM
    SERVICE_53              = STANDARD_SEC_CAPTURE
    SERVICE_54              = STANDARD_US
    SERVICE_55              = STANDARD_US_MF
    SERVICE_56              = STANDARD_US_MF_RETIRED
    SERVICE_57              = STANDARD_US_RETIRED
    SERVICE_58              = STANDARD_VIDEO_ENDOSCOPIC 
    SERVICE_59              = STANDARD_VIDEO_MICROSCOPIC
    SERVICE_60              = STANDARD_VIDEO_PHOTOGRAPHIC
    SERVICE_61              = STANDARD_VL_ENDOSCOPIC
    SERVICE_62              = STANDARD_VL_MICROSCOPIC
    SERVICE_63              = STANDARD_VL_PHOTOGRAPHIC
    SERVICE_64              = STANDARD_VL_SLIDE_MICROSCOPIC
    SERVICE_65              = STANDARD_VOI_LUT
    SERVICE_66              = STANDARD_WAVEFORM_12_LEAD_ECG
    SERVICE_67              = STANDARD_WAVEFORM_AMBULATORY_ECG
    SERVICE_68              = STANDARD_WAVEFORM_BASIC_VOICE_AU
    SERVICE_69              = STANDARD_WAVEFORM_CARDIAC_EP
    SERVICE_70              = STANDARD_WAVEFORM_GENERAL_ECG
    SERVICE_71              = STANDARD_WAVEFORM_HEMODYNAMIC
    SERVICE_72              = STANDARD_XRAY_ANGIO
    SERVICE_73              = STANDARD_XRAY_ANGIO_BIPLANE
    SERVICE_74              = STANDARD_XRAY_RF
    SERVICE_75              = STEREOMETRIC_RELATIONSHIP
    SERVICE_76              = ENHANCED_XA_IMAGE
    SERVICE_77              = ENHANCED_XRF_IMAGE
    SERVICE_78              = RAW_DATA
    SERVICE_79              = REAL_WORLD_VALUE_MAPPING
    SERVICE_80              = STANDARD_BLENDING_SOFTCOPY_PS
    SERVICE_81              = STANDARD_COLOR_SOFTCOPY_PS
    SERVICE_82              = STANDARD_ENCAPSULATED_PDF
    SERVICE_83              = STANDARD_PSEUDOCOLOR_SOFTCOPY_PS
    SERVICE_84              = STANDARD_RT_ION_BEAMS_TREAT
    SERVICE_85              = STANDARD_RT_ION_PLAN
    SERVICE_86              = DEFORMABLE_SPATIAL_REGISTRATION
    SERVICE_87              = SEGMENTATION
    SERVICE_88              = COMPOSITE_INSTANCE_ROOT_RET_MOVE

#
# This service list is used by the QR_GET_SCP sample application to 
# open an assocation up with the QR_GET_SCU.
#
[GET_SCP_Service_List]
    SERVICES_SUPPORTED      = 93     # Number of Services in list
    SERVICE_1               = PATIENT_STUDY_ONLY_QR_FIND
    SERVICE_2               = PATIENT_STUDY_ONLY_QR_MOVE
    SERVICE_3               = STUDY_ROOT_QR_FIND
    SERVICE_4               = STUDY_ROOT_QR_MOVE
    SERVICE_5               = PATIENT_ROOT_QR_FIND
    SERVICE_6               = PATIENT_ROOT_QR_MOVE
    SERVICE_7               = XRAY_RADIATION_DOSE_SR
    ROLE_7                  = BOTH
    SERVICE_8               = CHEST_CAD_SR
    ROLE_8                  = BOTH
    SERVICE_9               = ENHANCED_CT_IMAGE
    ROLE_9                  = BOTH
    SERVICE_10              = ENHANCED_MR_IMAGE
    ROLE_10                 = BOTH
    SERVICE_11              = KEY_OBJECT_SELECTION_DOC
    ROLE_11                 = BOTH
    SERVICE_12              = MAMMOGRAPHY_CAD_SR
    ROLE_12                 = BOTH
    SERVICE_13              = MR_SPECTROSCOPY
    ROLE_13                 = BOTH
    SERVICE_14              = PROCEDURE_LOG
    ROLE_14                 = BOTH
    SERVICE_15              = SC_MULTIFRAME_GRAYSCALE_BYTE
    ROLE_15                 = BOTH
    SERVICE_16              = SC_MULTIFRAME_GRAYSCALE_WORD
    ROLE_16                 = BOTH
    SERVICE_17              = SC_MULTIFRAME_SINGLE_BIT
    ROLE_17                 = BOTH
    SERVICE_18              = SC_MULTIFRAME_TRUE_COLOR
    ROLE_18                 = BOTH
    SERVICE_19              = SPATIAL_FIDUCIALS
    ROLE_19                 = BOTH
    SERVICE_20              = SPATIAL_REGISTRATION
    ROLE_20                 = BOTH
    SERVICE_21              = STANDARD_BASIC_TEXT_SR
    ROLE_21                 = BOTH
    SERVICE_22              = STANDARD_COMPREHENSIVE_SR
    ROLE_22                 = BOTH
    SERVICE_23              = STANDARD_CR
    ROLE_23                 = BOTH
    SERVICE_24              = STANDARD_CT
    ROLE_24                 = BOTH
    SERVICE_25              = STANDARD_CURVE
    ROLE_25                 = BOTH
    SERVICE_26              = STANDARD_DX_PRESENT
    ROLE_26                 = BOTH
    SERVICE_27              = STANDARD_DX_PROCESS
    ROLE_27                 = BOTH
    SERVICE_28              = STANDARD_ENHANCED_SR
    ROLE_28                 = BOTH
    SERVICE_29              = STANDARD_GRAYSCALE_SOFTCOPY_PS
    ROLE_29                 = BOTH
    SERVICE_30              = STANDARD_HARDCOPY_COLOR
    ROLE_30                 = BOTH
    SERVICE_31              = STANDARD_HARDCOPY_GRAYSCALE
    ROLE_31                 = BOTH
    SERVICE_32              = STANDARD_IO_PRESENT
    ROLE_32                 = BOTH
    SERVICE_33              = STANDARD_IO_PROCESS
    ROLE_33                 = BOTH
    SERVICE_34              = STANDARD_MG_PRESENT
    ROLE_34                 = BOTH
    SERVICE_35              = STANDARD_MG_PROCESS
    ROLE_35                 = BOTH
    SERVICE_36              = STANDARD_MODALITY_LUT
    ROLE_36                 = BOTH
    SERVICE_37              = STANDARD_MR
    ROLE_37                 = BOTH
    SERVICE_38              = STANDARD_NM
    ROLE_38                 = BOTH
    SERVICE_39              = STANDARD_NM_RETIRED
    ROLE_39                 = BOTH
    SERVICE_40              = STANDARD_OPHTHALMIC_16_BIT
    ROLE_40                 = BOTH
    SERVICE_41              = STANDARD_OPHTHALMIC_8_BIT
    ROLE_41                 = BOTH
    SERVICE_42              = STANDARD_OVERLAY
    ROLE_42                 = BOTH
    SERVICE_43              = STANDARD_PET
    ROLE_43                 = BOTH
    SERVICE_44              = STANDARD_PET_CURVE
    ROLE_44                 = BOTH
    SERVICE_45              = STANDARD_PRINT_STORAGE
    ROLE_45                 = BOTH
    SERVICE_46              = STANDARD_RT_BEAMS_TREAT
    ROLE_46                 = BOTH
    SERVICE_47              = STANDARD_RT_BRACHY_TREAT
    ROLE_47                 = BOTH
    SERVICE_48              = STANDARD_RT_DOSE
    ROLE_48                 = BOTH
    SERVICE_49              = STANDARD_RT_IMAGE
    ROLE_49                 = BOTH
    SERVICE_50              = STANDARD_RT_PLAN
    ROLE_50                 = BOTH
    SERVICE_51              = STANDARD_RT_STRUCTURE_SET
    ROLE_51                 = BOTH
    SERVICE_52              = STANDARD_RT_TREAT_SUM
    ROLE_52                 = BOTH
    SERVICE_53              = STANDARD_SEC_CAPTURE
    ROLE_53                 = BOTH
    SERVICE_54              = STANDARD_US
    ROLE_54                 = BOTH
    SERVICE_55              = STANDARD_US_MF
    ROLE_55                 = BOTH
    SERVICE_56              = STANDARD_US_MF_RETIRED
    ROLE_56                 = BOTH
    SERVICE_57              = STANDARD_US_RETIRED
    ROLE_57                 = BOTH
    SERVICE_58              = STANDARD_VIDEO_ENDOSCOPIC 
    ROLE_58                 = BOTH
    SERVICE_59              = STANDARD_VIDEO_MICROSCOPIC
    ROLE_59                 = BOTH
    SERVICE_60              = STANDARD_VIDEO_PHOTOGRAPHIC
    ROLE_60                 = BOTH
    SERVICE_61              = STANDARD_VL_ENDOSCOPIC
    ROLE_61                 = BOTH
    SERVICE_62              = STANDARD_VL_MICROSCOPIC
    ROLE_62                 = BOTH
    SERVICE_63              = STANDARD_VL_PHOTOGRAPHIC
    ROLE_63                 = BOTH
    SERVICE_64              = STANDARD_VL_SLIDE_MICROSCOPIC
    ROLE_64                 = BOTH
    SERVICE_65              = STANDARD_VOI_LUT
    ROLE_65                 = BOTH
    SERVICE_66              = STANDARD_WAVEFORM_12_LEAD_ECG
    ROLE_66                 = BOTH
    SERVICE_67              = STANDARD_WAVEFORM_AMBULATORY_ECG
    ROLE_67                 = BOTH
    SERVICE_68              = STANDARD_WAVEFORM_BASIC_VOICE_AU
    ROLE_68                 = BOTH
    SERVICE_69              = STANDARD_WAVEFORM_CARDIAC_EP
    ROLE_69                 = BOTH
    SERVICE_70              = STANDARD_WAVEFORM_GENERAL_ECG
    ROLE_70                 = BOTH
    SERVICE_71              = STANDARD_WAVEFORM_HEMODYNAMIC
    ROLE_71                 = BOTH
    SERVICE_72              = STANDARD_XRAY_ANGIO
    ROLE_72                 = BOTH
    SERVICE_73              = STANDARD_XRAY_ANGIO_BIPLANE
    ROLE_73                 = BOTH
    SERVICE_74              = STANDARD_XRAY_RF
    ROLE_74                 = BOTH
    SERVICE_75              = STEREOMETRIC_RELATIONSHIP
    ROLE_75                 = BOTH
    SERVICE_76              = ENHANCED_XA_IMAGE
    ROLE_76                 = BOTH
    SERVICE_77              = ENHANCED_XRF_IMAGE
    ROLE_77                 = BOTH
    SERVICE_78              = RAW_DATA
    ROLE_78                 = BOTH
    SERVICE_79              = REAL_WORLD_VALUE_MAPPING
    ROLE_79                 = BOTH
    SERVICE_80              = STANDARD_BLENDING_SOFTCOPY_PS
    ROLE_80                 = BOTH
    SERVICE_81              = STANDARD_COLOR_SOFTCOPY_PS
    ROLE_81                 = BOTH
    SERVICE_82              = STANDARD_ENCAPSULATED_PDF
    ROLE_82                 = BOTH
    SERVICE_83              = STANDARD_PSEUDOCOLOR_SOFTCOPY_PS
    ROLE_83                 = BOTH
    SERVICE_84              = STANDARD_RT_ION_BEAMS_TREAT
    ROLE_84                 = BOTH
    SERVICE_85              = STANDARD_RT_ION_PLAN
    ROLE_85                 = BOTH
    SERVICE_86              = DEFORMABLE_SPATIAL_REGISTRATION
    ROLE_86                 = BOTH
    SERVICE_87              = SEGMENTATION
    ROLE_87                 = BOTH
    SERVICE_88              = PATIENT_STUDY_ONLY_QR_GET
    ROLE_88                 = BOTH
    SERVICE_89              = STUDY_ROOT_QR_GET
    SERVICE_90              = PATIENT_ROOT_QR_GET
    SERVICE_91              = COMPOSITE_INSTANCE_ROOT_RET_MOVE
    SERVICE_92              = COMPOSITE_INSTANCE_ROOT_RET_GET    
    SERVICE_93              = COMPOSITE_INST_RET_NO_BULK_GET        

[Query_SCU_Service_List]
    SERVICES_SUPPORTED      = 7      # Number of Services in list
    SERVICE_1               = PATIENT_STUDY_ONLY_QR_FIND
    SERVICE_2               = PATIENT_STUDY_ONLY_QR_MOVE
    SERVICE_3               = STUDY_ROOT_QR_FIND
    EXT_NEG_INFO_3          = 0x01   # This example would propose relational queries in the DICOM extended negotiation for Study Root QR Finds
    SERVICE_4               = STUDY_ROOT_QR_MOVE
    SERVICE_5               = PATIENT_ROOT_QR_FIND
    SERVICE_6               = PATIENT_ROOT_QR_MOVE
    SERVICE_7               = COMPOSITE_INSTANCE_ROOT_RET_MOVE

#
# This service list is used by the QR_GET_SCU sample application to 
# open an assocation up with the QR_GET_SCP.
#
[GET_SCU_Service_List]
    SERVICES_SUPPORTED      = 91       # Number of Services in list
    SERVICE_1               = STORAGE_COMMITMENT_PUSH
    SERVICE_2               = CHEST_CAD_SR
    ROLE_2                  = SCP 
    SERVICE_3               = ENHANCED_CT_IMAGE
    ROLE_3                  = SCP
    SERVICE_4               = ENHANCED_MR_IMAGE
    ROLE_4                  = SCP
    SERVICE_5               = KEY_OBJECT_SELECTION_DOC
    ROLE_5                  = SCP
    SERVICE_6               = MAMMOGRAPHY_CAD_SR
    ROLE_6                  = SCP
    SERVICE_7               = MR_SPECTROSCOPY
    ROLE_7                  = SCP
    SERVICE_8               = PROCEDURE_LOG
    ROLE_8                  = SCP
    SERVICE_9               = SC_MULTIFRAME_GRAYSCALE_BYTE    
    ROLE_9                  = SCP
    SERVICE_10              = SC_MULTIFRAME_GRAYSCALE_WORD
    ROLE_10                 = SCP
    SERVICE_11              = SC_MULTIFRAME_SINGLE_BIT
    ROLE_11                 = SCP
    SERVICE_12              = SC_MULTIFRAME_TRUE_COLOR
    ROLE_12                 = SCP
    SERVICE_13              = SPATIAL_FIDUCIALS
    ROLE_13                 = SCP
    SERVICE_14              = SPATIAL_REGISTRATION
    ROLE_14                 = SCP
    SERVICE_15              = STANDARD_BASIC_TEXT_SR
    ROLE_15                 = SCP
    SERVICE_16              = STANDARD_COMPREHENSIVE_SR
    ROLE_16                 = SCP
    SERVICE_17              = STANDARD_CR
    ROLE_17                 = SCP
    SERVICE_18              = STANDARD_CT
    ROLE_18                 = SCP
    SYNTAX_LIST_18          = Uncompressed_Syntax_List    # This is provided as an example and can be removed
    SERVICE_19              = STANDARD_CURVE
    ROLE_19                 = SCP
    SERVICE_20              = STANDARD_DX_PRESENT
    ROLE_20                 = SCP
    SERVICE_21              = STANDARD_DX_PROCESS
    ROLE_21                 = SCP
    SERVICE_22              = STANDARD_ENHANCED_SR
    ROLE_22                 = SCP
    SERVICE_23              = STANDARD_GRAYSCALE_SOFTCOPY_PS
    ROLE_23                 = SCP
    SERVICE_24              = STANDARD_HARDCOPY_COLOR
    ROLE_24                 = SCP
    SERVICE_25              = STANDARD_HARDCOPY_GRAYSCALE
    ROLE_25                 = SCP
    SERVICE_26              = STANDARD_IO_PRESENT
    ROLE_26                 = SCP
    SERVICE_27              = STANDARD_IO_PROCESS
    ROLE_27                 = SCP
    SERVICE_28              = STANDARD_MG_PRESENT
    ROLE_28                 = SCP
    SERVICE_29              = STANDARD_MG_PROCESS
    ROLE_29                 = SCP
    SERVICE_30              = STANDARD_MODALITY_LUT
    ROLE_30                 = SCP
    SERVICE_31              = STANDARD_MR
    ROLE_31                 = SCP
    SERVICE_32              = STANDARD_NM
    ROLE_32                 = SCP
    SERVICE_33              = STANDARD_NM_RETIRED
    ROLE_33                 = SCP
    SERVICE_34              = STANDARD_OPHTHALMIC_16_BIT
    ROLE_34                 = SCP
    SERVICE_35              = STANDARD_OPHTHALMIC_8_BIT
    ROLE_35                 = SCP
    SERVICE_36              = STANDARD_OVERLAY
    ROLE_36                 = SCP
    SERVICE_37              = STANDARD_PET
    ROLE_37                 = SCP
    SERVICE_38              = STANDARD_PET_CURVE
    ROLE_38                 = SCP
    SERVICE_39              = STANDARD_PRINT_STORAGE
    ROLE_39                 = SCP
    SERVICE_40              = STANDARD_RT_BEAMS_TREAT
    ROLE_40                 = SCP
    SERVICE_41              = STANDARD_RT_BRACHY_TREAT
    ROLE_41                 = SCP
    SERVICE_42              = STANDARD_RT_DOSE
    ROLE_42                 = SCP
    SERVICE_43              = STANDARD_RT_IMAGE
    ROLE_43                 = SCP
    SERVICE_44              = STANDARD_RT_PLAN
    ROLE_44                 = SCP
    SERVICE_45              = STANDARD_RT_STRUCTURE_SET
    ROLE_45                 = SCP
    SERVICE_46              = STANDARD_RT_TREAT_SUM
    ROLE_46                 = SCP
    SERVICE_47              = STANDARD_SEC_CAPTURE
    ROLE_47                 = SCP
    SERVICE_48              = STANDARD_US
    ROLE_48                 = SCP
    SERVICE_49              = STANDARD_US_MF
    ROLE_49                 = SCP
    SERVICE_50              = STANDARD_US_MF_RETIRED
    ROLE_50                 = SCP
    SERVICE_51              = STANDARD_US_RETIRED
    ROLE_51                 = SCP
    SERVICE_52              = STANDARD_VIDEO_ENDOSCOPIC 
    ROLE_52                 = SCP
    SERVICE_53              = STANDARD_VIDEO_MICROSCOPIC
    ROLE_53                 = SCP
    SERVICE_54              = STANDARD_VIDEO_PHOTOGRAPHIC
    ROLE_54                 = SCP
    SERVICE_55              = STANDARD_VL_ENDOSCOPIC
    ROLE_55                 = SCP
    SERVICE_56              = STANDARD_VL_MICROSCOPIC
    ROLE_56                 = SCP
    SERVICE_57              = STANDARD_VL_PHOTOGRAPHIC
    ROLE_57                 = SCP
    SERVICE_58              = STANDARD_VL_SLIDE_MICROSCOPIC
    ROLE_58                 = SCP
    SERVICE_59              = STANDARD_VOI_LUT
    ROLE_59                 = SCP
    SERVICE_60              = STANDARD_WAVEFORM_12_LEAD_ECG
    ROLE_60                 = SCP
    SERVICE_61              = STANDARD_WAVEFORM_AMBULATORY_ECG
    ROLE_61                 = SCP
    SERVICE_62              = STANDARD_WAVEFORM_BASIC_VOICE_AU
    ROLE_62                 = SCP
    SERVICE_63              = STANDARD_WAVEFORM_CARDIAC_EP
    ROLE_63                 = SCP
    SERVICE_64              = STANDARD_WAVEFORM_GENERAL_ECG
    ROLE_64                 = SCP
    SERVICE_65              = STANDARD_WAVEFORM_HEMODYNAMIC
    ROLE_65                 = SCP
    SERVICE_66              = STANDARD_XRAY_ANGIO
    ROLE_66                 = SCP
    SERVICE_67              = STANDARD_XRAY_ANGIO_BIPLANE
    ROLE_67                 = SCP
    SERVICE_68              = STANDARD_XRAY_RF
    ROLE_68                 = SCP
    SERVICE_69              = STEREOMETRIC_RELATIONSHIP
    ROLE_69                 = SCP
    SERVICE_70              = ENHANCED_XA_IMAGE
    ROLE_70                 = SCP
    SERVICE_71              = ENHANCED_XRF_IMAGE
    ROLE_71                 = SCP
    SERVICE_72              = RAW_DATA
    ROLE_72                 = SCP
    SERVICE_73              = REAL_WORLD_VALUE_MAPPING
    ROLE_73                 = SCP
    SERVICE_74              = STANDARD_BLENDING_SOFTCOPY_PS
    ROLE_74                 = SCP
    SERVICE_75              = STANDARD_COLOR_SOFTCOPY_PS
    ROLE_75                 = SCP
    SERVICE_76              = STANDARD_ENCAPSULATED_PDF
    ROLE_76                 = SCP
    SERVICE_77              = STANDARD_PSEUDOCOLOR_SOFTCOPY_PS
    ROLE_77                 = SCP
    SERVICE_78              = STANDARD_RT_ION_BEAMS_TREAT
    ROLE_78                 = SCP
    SERVICE_79              = STANDARD_RT_ION_PLAN
    ROLE_79                 = SCP
    SERVICE_80              = XRAY_RADIATION_DOSE_SR
    ROLE_80                 = SCP
    SERVICE_81              = DEFORMABLE_SPATIAL_REGISTRATION
    ROLE_81                 = SCP
    SERVICE_82              = SEGMENTATION
    ROLE_82                 = SCP
    
    SERVICE_83              = PATIENT_STUDY_ONLY_QR_FIND
    SERVICE_84              = STUDY_ROOT_QR_FIND
    SERVICE_85              = PATIENT_ROOT_QR_FIND
    SERVICE_86              = PATIENT_STUDY_ONLY_QR_GET
    SERVICE_87              = STUDY_ROOT_QR_GET
    SERVICE_88              = PATIENT_ROOT_QR_GET
    SERVICE_89              = COMPOSITE_INSTANCE_ROOT_RET_MOVE
    SERVICE_90              = COMPOSITE_INSTANCE_ROOT_RET_GET    
    SERVICE_91              = COMPOSITE_INST_RET_NO_BULK_GET        

[Print_Service_List]
    SERVICES_SUPPORTED      = 2       # Number of Services in list
    SERVICE_1               = BASIC_GRAYSCALE_PRINT_MANAGEMENT
    SERVICE_2               = PRINT_JOB

[Worklist_Service_List]
    SERVICES_SUPPORTED      = 2       # Number of Services in list
    SERVICE_1               = MODALITY_WORKLIST_FIND
    SERVICE_2               = PERFORMED_PROCEDURE_STEP

[Verification_Service_List]
    SERVICES_SUPPORTED      = 1
    SERVICE_1               = STANDARD_ECHO

[UPS_SERVICE_LIST]
    SERVICES_SUPPORTED      = 4       # Number of Services in list
    SERVICE_1               = UPS_PULL_SOP
    SERVICE_2               = UPS_WATCH_SOP
    SERVICE_3               = UPS_PUSH_SOP
    SERVICE_4               = UPS_EVENT_SOP

[UPS_SERVICE_LIST_RQ]
    SERVICES_SUPPORTED      = 1       # Number of Services in list
    SERVICE_1               = UPS_EVENT_SOP
    ROLE_1                  = SCP

[UPS_SERVICE_LIST_ACC]
    SERVICES_SUPPORTED      = 1       # Number of Services in list
    SERVICE_1               = UPS_EVENT_SOP
    ROLE_1                  = SCU

[AETitles_Service_List]
    SERVICES_SUPPORTED      = 5    
    SERVICE_1               = STANDARD_MR
    SYNTAX_LIST_1           = 01_Syntax_List
    SERVICE_2               = STANDARD_CT
    SYNTAX_LIST_2           = 02_Syntax_List
    SERVICE_3               = STANDARD_XRAY_RF
    SYNTAX_LIST_3           = 03_Syntax_List
    SERVICE_4               = STANDARD_XRAY_ANGIO
    SYNTAX_LIST_4           = 06_Syntax_List
    SERVICE_5               = STANDARD_XRAY_ANGIO_BIPLANE
    SYNTAX_LIST_5           = 07_Syntax_List

# Each [<syntax_list_name>] section must contain a SYNTAXS_SUPPORTED
#       parameter to specify the number of transfer syntaxes in the 
#       list; plus one parameter for each syntax, of the following format:
#
#       SYNTAX_n = <transfer syntax name>
#
#       where:
#               SYNTAX_n        the character string "SYNTAX_n", with n 
#                               replaced by the number of the transfer 
#                               syntaxes being named.
#               <transfer syntax name>  is the name of one of the transfer
#                               syntaxes supported by your MergeCOM-3 system.  
#                               It must match one of the following DICOM transfer 
#                               syntaxes:
#
#                       IMPLICIT_LITTLE_ENDIAN
#                       IMPLICIT_BIG_ENDIAN
#                       EXPLICIT_LITTLE_ENDIAN
#                       EXPLICIT_BIG_ENDIAN
#                       DEFLATED_EXPLICIT_LITTLE_ENDIAN
#                       RLE
#                       JPEG_BASELINE
#                       JPEG_EXTENDED_2_4
#                       JPEG_EXTENDED_3_5
#                       JPEG_SPEC_NON_HIER_6_8
#                       JPEG_SPEC_NON_HIER_7_9
#                       JPEG_FULL_PROG_NON_HIER_10_12
#                       JPEG_FULL_PROG_NON_HIER_11_13
#                       JPEG_LOSSLESS_NON_HIER_14
#                       JPEG_LOSSLESS_NON_HIER_15
#                       JPEG_EXTENDED_HIER_16_18
#                       JPEG_EXTENDED_HIER_17_19
#                       JPEG_SPEC_HIER_20_22
#                       JPEG_SPEC_HIER_21_23
#                       JPEG_FULL_PROG_HIER_24_26
#                       JPEG_FULL_PROG_HIER_25_27
#                       JPEG_LOSSLESS_HIER_28
#                       JPEG_LOSSLESS_HIER_29
#                       JPEG_LOSSLESS_HIER_14
#                       JPEG_LS_LOSSLESS
#                       JPEG_LS_LOSSY
#                       JPEG_2000
#                       JPEG_2000_LOSSLESS_ONLY
#                       JPIP_REFERENCED
#                       JPIP_REFERENCED_DEFLATE
#                       MPEG2_MPML
#                       MPEG2_MPHL
#                       MPEG4_AVC_H264_HP_LEVEL_4_1
#                       MPEG4_AVC_H264_BDC_HP_LEVEL_4_1
#                       MPEG4_AVC_H264_HP_LEVEL_4_2_2D
#                       MPEG4_AVC_H264_HP_LEVEL_4_2_3D
#                       MPEG4_AVC_H264_STEREO_HP_LEVEL_4_2
#                       HEVC_H265_MP_LEVEL_5_1
#                       HEVC_H265_M10P_LEVEL_5_1
#                       PRIVATE_SYNTAX_1
#                       PRIVATE_SYNTAX_2
#
# Note that the order that these transfer syntaxes are listed
# defines how SCP applications using the tool kit will select 
# them during association negotiation.  The tool kit will place 
# the highest priority on the first syntax and decreasing 
# priority on following syntaxes in the transfer syntax list.

#
# Example syntax list used by the Storage_Service_List above.
#
[Uncompressed_Syntax_List]
    SYNTAXES_SUPPORTED      = 3       # Number of Syntaxes in list
    SYNTAX_1                = EXPLICIT_LITTLE_ENDIAN
    SYNTAX_2                = EXPLICIT_BIG_ENDIAN
    SYNTAX_3                = IMPLICIT_LITTLE_ENDIAN

[01_Syntax_List]
    SYNTAXES_SUPPORTED      = 1
    SYNTAX_1                = IMPLICIT_LITTLE_ENDIAN

[02_Syntax_List]
    SYNTAXES_SUPPORTED      = 1
    SYNTAX_1                = RLE

[03_Syntax_List]
    SYNTAXES_SUPPORTED      = 1
    SYNTAX_1                = EXPLICIT_LITTLE_ENDIAN

[04_Syntax_List]
    SYNTAXES_SUPPORTED      = 1
    SYNTAX_1                = EXPLICIT_BIG_ENDIAN

[05_Syntax_List]
    SYNTAXES_SUPPORTED      = 1
    SYNTAX_1                = JPEG_BASELINE

[06_Syntax_List]
    SYNTAXES_SUPPORTED      = 1
    SYNTAX_1                = JPEG_EXTENDED_2_4

[07_Syntax_List]
    SYNTAXES_SUPPORTED      =1
    SYNTAX_1                = JPEG_LOSSLESS_HIER_14

#
# The following two syntax lists are supplied as an example of how to 
# define syntax lists for compressed transfer syntaxes.
# Only the default lossless and lossy compression syntaxes have been included.
#
[Compressed_Syntax_List]
    SYNTAXES_SUPPORTED      = 3       # Number of Syntaxes in list
    SYNTAX_1                = JPEG_LOSSLESS_HIER_14
    SYNTAX_2                = JPEG_BASELINE  
    SYNTAX_3                = JPEG_EXTENDED_2_4      

[Full_Syntax_List]
    SYNTAXES_SUPPORTED      = 6       # Number of Syntaxes in list
    SYNTAX_1                = JPEG_LOSSLESS_HIER_14
    SYNTAX_2                = JPEG_BASELINE  
    SYNTAX_3                = JPEG_EXTENDED_2_4      
    SYNTAX_4                = EXPLICIT_LITTLE_ENDIAN
    SYNTAX_5                = EXPLICIT_BIG_ENDIAN
    SYNTAX_6                = IMPLICIT_LITTLE_ENDIAN

