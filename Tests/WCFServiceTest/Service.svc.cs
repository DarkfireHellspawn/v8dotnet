﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using V8.Net;

/* V8.NET integration into ASP.NET:
 * 1. Create a folder specifically named "V8.NET", or change the name using 'Loader.AlternateRootSubPath'.
 *    Note: This test project has a PRE-Build event that copies files to "$(ProjectDir)V8.NET" - this must match 'Loader.AlternateRootSubPath'.
 *    Note: 'Loader.AlternateRootSubPath' is set to "V8.NET"  by default.
 * 2. For all DLLS in the "$(ProjectDir)V8.NET" folder, change "Copy to Output Directory" to "Copy if newer".  This should also
 *    tell Visual Studio that this content is required for the application.
 * 3. Set any "$(ProjectDir)V8.NET\*.DLL" files that you have REFERENCED to "Do not copy" - the build action will copy referenced DLLs by default.
 * 
 * When setup correctly, Visual Studio will replicate the folder structure for the "V8.NET" folder into the 'bin' folder, and the referenced DLLs will end
 * up in the ROOT of the 'bin' folder.  Thus, you will have this folder structure:
 * - bin\V8.Net.dll
 * - bin\V8.Net.SharedTypes.dll (if referenced)
 * - bin\V8.Net\*.dll
 * V8.Net.dll will look for the "V8.NET" folder in the 'bin' folder for the other DLLs, and if found, will use that instead to locate dependent libraries.
 * 
 * Note: ASP.NET may shadow copy some DLLs in the 'bin' folder (http://goo.gl/vXbwGp).  This means that those DLLs may end up elsewhere in a temporary folder
 * during runtime.
 */

namespace WCFServiceTest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        public string GetData(int value)
        {
            // V8Engine.ASPBINSubFolderName = "V8.NET"; // It is already "V8.NET" by default, so just delete this line if not needed.  Please see integration steps at the top for more details.
            var engine = new V8Engine();
            var result = engine.Execute("'You entered: '+" + value, "V8.NET Web Service Test");
            return result.AsString;
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
