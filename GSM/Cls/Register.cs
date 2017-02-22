using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using License; 
namespace GSM.Cls
{
    class Register
    {
        /*** Check if a valid license file is available. ***/
        public bool IsValidLicenseAvailable()
        {
            return License.Status.Licensed;
        }

        /*** Read additonal license information from a license license ***/
        public void ReadAdditonalLicenseInformation(out string[] strmsg)
        {
            int count = License.Status.KeyValueList.Count;
            strmsg = new string[count];
            /* Check first if a valid license file is found */
            if (License.Status.Licensed)
            {
                /* Read additional license information */
                for (int i = 0; i < count; i++)
                {
                    strmsg[i] = License.Status.KeyValueList.GetKey(i).ToString() +":"+ License.Status.KeyValueList.GetByIndex(i).ToString(); ;
                }
            } 
        }

        /*** Check the license status of Evaluation Lock ***/
        public void CheckEvaluationLock()
        {
            bool lock_enabled = License.Status.Evaluation_Lock_Enabled;
            License.EvaluationType ev_type = License.Status.Evaluation_Type;
            int time = License.Status.Evaluation_Time;
            int time_current = License.Status.Evaluation_Time_Current;
        }


        /*** Check the license status of Expiration Date Lock ***/
        public System.DateTime CheckExpirationDateLock()
        {
            bool lock_enabled = License.Status.Expiration_Date_Lock_Enable;
            return License.Status.Expiration_Date;
        }


        /*** Check the license status of Number Of Uses Lock ***/
        public void CheckNumberOfUsesLock()
        {
            bool lock_enabled = License.Status.Number_Of_Uses_Lock_Enable;
            int max_uses = License.Status.Number_Of_Uses;
            int current_uses = License.Status.Number_Of_Uses_Current;
        }


        /*** Check the license status of Number Of Instances Lock ***/
        public void CheckNumberOfInstancesLock()
        {
            bool lock_enabled = License.Status.Number_Of_Instances_Lock_Enable;
            int max_instances = License.Status.Number_Of_Instances;
        }


        /*** Check the license status of Hardware Lock ***/
        public string CheckHardwareLock()
        {
            bool lock_enabled = License.Status.Hardware_Lock_Enabled;
            string lic_hardware_id=string.Empty;
            if (lock_enabled)
            {
                /* Get Hardware ID which is stored inside the license file */
                lic_hardware_id = License.Status.License_HardwareID;
            }
            return lic_hardware_id;
        }


        /*** Get Hardware ID of the current machine ***/
        public string GetHardwareID()
        {
            return License.Status.HardwareID;
        }


        /*** Compare current Hardware ID with Hardware ID stored in License File ***/
        public bool CompareHardwareID()
        {
            if (License.Status.HardwareID == License.Status.License_HardwareID)
                return true;
            else
                return false;
        }


        /*** Invalidate the license. Please note, your protected software does not accept a license file anymore! ***/
        public void InvalidateLicense()
        {
            string confirmation_code = License.Status.InvalidateLicense();
        }


        /*** Check if a confirmation code is valid ***/
        public bool CheckConfirmationCode(string confirmation_code)
        {
            return License.Status.CheckConfirmationCode(License.Status.HardwareID,
            confirmation_code);
        }


        /*** Reactivate an invalidated license. ***/
        public bool ReactivateLicense(string reactivation_code)
        {
            return License.Status.ReactivateLicense(reactivation_code);
        }


        /*** Load the license. ***/
        public void LoadLicense(string filename)
        {
            License.Status.LoadLicense(filename);
        }

        /*** Load the license. ***/
        public void LoadLicense(byte[] license)
        {
            License.Status.LoadLicense(license);
        }

        /*** Get the license. ***/
        public byte[] GetLicense()
        {
            return License.Status.License;
        }
     
    }
}
