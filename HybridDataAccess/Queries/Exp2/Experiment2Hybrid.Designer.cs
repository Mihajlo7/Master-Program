﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HybridDataAccess.Queries.Exp2 {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Experiment2Hybrid {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Experiment2Hybrid() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HybridDataAccess.Queries.Exp2.Experiment2Hybrid", typeof(Experiment2Hybrid).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Department (id,name,location,teams) VALUES (@Id,@Name,@Location,@TeamsWithEmployees);
        ///
        ///WITH cte AS(
        ///	SELECT id,[key],teams,value 
        ///	FROM Department CROSS APPLY OPENJSON(teams)
        ///	WHERE JSON_VALUE(value,&apos;$.Id&apos;)=@TeamId
        ///)
        ///UPDATE cte
        ///SET teams= JSON_MODIFY(teams,&apos;$[&apos;+cte.[key]+&apos;].Employees&apos;,JSON_QUERY(@Employees));
        ///.
        /// </summary>
        internal static string Insert {
            get {
                return ResourceManager.GetString("Insert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- 0.
        ///SELECT * FROM Department;
        ///-- 1.
        ///SELECT 
        ///    d.id AS DepartementId,
        ///    d.name AS DepartmentName,
        ///    d.location AS DepartmentLocation,
        ///    CAST(JSON_VALUE(t.value, &apos;$.Id&apos;) AS BIGINT) AS TeamId,
        ///    JSON_VALUE(t.value, &apos;$.Name&apos;) AS TeamName,
        ///    JSON_VALUE(t.value, &apos;$.Status&apos;) AS TeamStatus,
        ///    JSON_VALUE(t.value, &apos;$.Description&apos;) AS TeamDescription,
        ///    CAST(JSON_VALUE(t.value, &apos;$.Lead.Id&apos;) AS BIGINT) AS LeadId,
        ///    JSON_VALUE(t.value, &apos;$.Lead.FirstName&apos;) AS LeadFirstName,
        ///    JSON_VALUE [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Select {
            get {
                return ResourceManager.GetString("Select", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- Drop table if exists
        ///ALTER TABLE Employee DROP CONSTRAINT IF EXISTS fk_employee_team;
        ///ALTER TABLE Team DROP CONSTRAINT IF EXISTS fk_team_department;
        ///DROP TABLE IF EXISTS Department;
        ///-- Create table Department
        ///CREATE TABLE Department(
        ///	id BIGINT PRIMARY KEY,
        ///	name NVARCHAR(255) NULL,
        ///	location NVARCHAR(255) NULL,
        ///	teams NVARCHAR(MAX) NULL
        ///)
        ///.
        /// </summary>
        internal static string Tables {
            get {
                return ResourceManager.GetString("Tables", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///---------------------------------------
        ///-- 0.
        ///WITH cte AS(
        ///	SELECT [key],teams
        ///	FROM Department CROSS APPLY OPENJSON(teams)
        ///	WHERE JSON_VALUE(value,&apos;$.Id&apos;)=2
        ///)
        ///UPDATE cte
        ///SET teams= JSON_MODIFY(teams,&apos;$[&apos;+cte.[key]+&apos;].Status&apos;,&apos;Neki Moj&apos;);
        ///
        ///---------------------------------------
        ///-- 1.
        ///WITH cte AS(
        ///	SELECT t.[key] teamKey, e.[key] empKey, teams
        ///	FROM Department CROSS APPLY OPENJSON(teams) t CROSS APPLY OPENJSON(JSON_QUERY(value,&apos;$.Employees&apos;)) e
        ///	WHERE JSON_VALUE(e.value,&apos;$.Id&apos;)=@Id
        ///)
        ///UPDA [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Update {
            get {
                return ResourceManager.GetString("Update", resourceCulture);
            }
        }
    }
}
