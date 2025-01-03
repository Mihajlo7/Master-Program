﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HybridDataAccess.Queries {
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
    internal class Experiment1Hybrid {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Experiment1Hybrid() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HybridDataAccess.Queries.Experiment1Hybrid", typeof(Experiment1Hybrid).Assembly);
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
        ///   Looks up a localized string similar to INSERT INTO Task(id,name,description,priority,deadline,status,responsible,supervisor,employees)
        ///VALUES(@TaskId,@TaskName,@TaskDescription,@TaskPriority,@TaskDeadline,@TaskStatus,@Responsible,@Supervisor,@Employees);
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
        ///   Looks up a localized string similar to -- 1. Proecure for update Employee Tasks
        ///CREATE  PROC UpdateTasksFromOneEmployeeToAnother
        ///	@FromEmployee BIGINT,
        ///	@ToEmployee BIGINT
        ///AS
        ///BEGIN
        ///	DECLARE @TaskId BIGINT;
        ///	
        ///	
        ///	DECLARE id_cursor CURSOR FOR 
        ///	SELECT id FROM Task
        ///	WHERE id IN (SELECT id FROM Task t1 CROSS APPLY OPENJSON(t1.employees) t1e WHERE JSON_VALUE(t1e.value,&apos;$.Employee.Id&apos;)=@FromEmployee)
        ///
        ///	OPEN id_cursor
        ///	FETCH NEXT FROM id_cursor INTO @TaskId;
        ///
        ///	WHILE @@FETCH_STATUS=0
        ///		BEGIN
        ///		-- Part 1 --
        ///			UPDATE Task
        ///			SET employ [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Procedures {
            get {
                return ResourceManager.GetString("Procedures", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- 1. All With Serialization in code
        ///SELECT t.id TaskId,t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline,t.status TaskStatus, t.responsible TaskResponsible,t.supervisor TaskSupervisor,t.employees TaskEmployees
        ///FROM dbo.Task t;
        ///-- 2. All With Serialization on database
        ///SELECT t.id TaskId,t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline,t.status TaskStatus,
        ///r.Id ResponsibleId, r.email ResponsibleEmail, r.firstNam [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Select {
            get {
                return ResourceManager.GetString("Select", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP TABLE IF EXISTS Task;
        ///
        ///DROP PROC IF EXISTS UpdateTasksFromOneEmployeeToAnother;
        ///DROP PROC IF EXISTS UpdateEmployeePhoneById;
        ///DROP PROC IF EXISTS UpdateEmployeePhoneByEmail;
        ///
        ///
        ///CREATE TABLE Task(
        ///	id BIGINT CONSTRAINT task_id PRIMARY KEY,
        ///	name NVARCHAR(100) NULL,
        ///	description NVARCHAR(1000) NULL,
        ///	priority INT DEFAULT 0,
        ///	deadline DATETIME  DEFAULT GETDATE(),
        ///	status NVARCHAR(10) DEFAULT &apos;Unknown&apos;,
        ///	responsible NVARCHAR(MAX) NULL,
        ///	supervisor NVARCHAR(MAX) NULL,
        ///	employees NVARCHAR(MAX)  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Tables {
            get {
                return ResourceManager.GetString("Tables", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- 1. Update Task expired and pedning
        ///UPDATE dbo.Task SET Status = &apos;Cancelled&apos; WHERE Deadline &lt; GETDATE() AND Status = &apos;Pending&apos;;
        ///-- 2. Update deadline for priority and deadline
        ///UPDATE dbo.Task
        ///SET Deadline = DATEADD(DAY, 5, Deadline) 
        ///WHERE Priority &lt; 4 
        ///  AND Deadline BETWEEN GETDATE() AND DATEADD(DAY, 3, GETDATE());
        ///-- 3. Update Deadline by LastName
        ///UPDATE dbo.Task
        ///SET deadline= DATEADD(DAY,3,deadline)
        ///WHERE JSON_VALUE(responsible,&apos;$.LastName&apos;) LIKE &apos;M%&apos;;
        ///-- 4. Update Deadline By Responsible T [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Update {
            get {
                return ResourceManager.GetString("Update", resourceCulture);
            }
        }
    }
}
