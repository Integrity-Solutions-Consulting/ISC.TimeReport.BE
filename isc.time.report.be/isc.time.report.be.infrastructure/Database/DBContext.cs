using isc.time.report.be.domain.Entity.Activities;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Clients;
using isc.time.report.be.domain.Entity.DailyActivities;
using isc.time.report.be.domain.Entity.Employees;
using isc.time.report.be.domain.Entity.Genders;
using isc.time.report.be.domain.Entity.Historycs;
using isc.time.report.be.domain.Entity.Holidays;
using isc.time.report.be.domain.Entity.IdentificationTypes;
using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Entity.Modules;
using isc.time.report.be.domain.Entity.Nationalities;
using isc.time.report.be.domain.Entity.Permisions;
using isc.time.report.be.domain.Entity.Persons;
using isc.time.report.be.domain.Entity.Positions;
using isc.time.report.be.domain.Entity.Projects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Database
{
    
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Nationality>(entity =>
            {
                entity.ToTable("Nationality");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("NationalityID");
                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Genders");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("GenderID");
                entity.Property(e => e.GenderCode).HasColumnName("gender_code");
                entity.Property(e => e.GenderName).HasColumnName("gender_name");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");
            });

            modelBuilder.Entity<IdentificationType>(entity =>
            {
                entity.ToTable("IdentificationTypes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdentificationTypeID");
                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("Positions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("PositionID");
                entity.Property(e => e.PositionName).HasColumnName("position_name");
                entity.Property(e => e.Department).HasColumnName("department");
                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Persons");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("PersonID");

                entity.Property(e => e.GenderID).HasColumnName("GenderID");
                entity.Property(e => e.NationalityId).HasColumnName("NationalityID");
                entity.Property(e => e.IdentificationTypeId).HasColumnName("IdentificationTypeid");
                entity.Property(e => e.IdentificationNumber).HasColumnName("identification_number");
                entity.Property(e => e.PersonType).HasColumnName("person_type");
                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.BirthDate).HasColumnName("birth_date");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");

                entity.HasIndex(e => new { e.IdentificationTypeId, e.IdentificationNumber }).IsUnique().HasDatabaseName("UQ_Persons_Identification");

                entity.HasOne(e => e.Gender).WithMany().HasForeignKey(e => e.GenderID);
                entity.HasOne(e => e.Nationality).WithMany().HasForeignKey(e => e.NationalityId);
                entity.HasOne(e => e.IdentificationType).WithMany().HasForeignKey(e => e.IdentificationTypeId);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ClientID");

                entity.Property(e => e.PersonID).HasColumnName("PersonID");
                entity.Property(e => e.TradeName).HasColumnName("trade_name");
                entity.Property(e => e.LegalName).HasColumnName("legal_name");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");

                entity.HasOne(e => e.Person).WithMany().HasForeignKey(e => e.PersonID);
            });

            modelBuilder.Entity<ProjectStatus>(entity =>
            {
                entity.ToTable("ProjectStatus");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ProjectStatusID");

                entity.Property(e => e.StatusCode).HasColumnName("status_code");
                entity.Property(e => e.StatusName).HasColumnName("status_name");
                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ProjectID");

                entity.Property(e => e.ClientID).HasColumnName("ClientID");
                entity.Property(e => e.ProjectStatusID).HasColumnName("ProjectStatusID");
                entity.Property(e => e.Code).HasColumnName("code");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.StartDate).HasColumnName("start_date");
                entity.Property(e => e.EndDate).HasColumnName("end_date");
                entity.Property(e => e.ActualStartDate).HasColumnName("actual_start_date");
                entity.Property(e => e.ActualEndDate).HasColumnName("actual_end_date");
                entity.Property(e => e.Budget).HasColumnName("budget");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");

                entity.HasOne(e => e.Client).WithMany().HasForeignKey(e => e.ClientID);
                entity.HasOne(e => e.ProjectStatus).WithMany().HasForeignKey(e => e.ProjectStatusID);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("EmployeeID");

                entity.Property(e => e.PersonID).HasColumnName("PersonID");
                entity.Property(e => e.PositionID).HasColumnName("PositionID");
                entity.Property(e => e.EmployeeCode).HasColumnName("employee_code");
                entity.Property(e => e.HireDate).HasColumnName("hire_date");
                entity.Property(e => e.TerminationDate).HasColumnName("termination_date");
                entity.Property(e => e.ContractType).HasColumnName("contract_type");
                entity.Property(e => e.Department).HasColumnName("department");
                entity.Property(e => e.CorporateEmail).HasColumnName("corporate_email");
                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");

                entity.HasOne(e => e.Person).WithMany().HasForeignKey(e => e.PersonID);
                entity.HasOne(e => e.Position).WithMany().HasForeignKey(e => e.PositionID);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("UserID");

                entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
                entity.Property(e => e.Username).HasColumnName("username");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.LastLogin).HasColumnName("last_login");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.MustChangePassword).HasColumnName("must_change_password");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");

                entity.HasOne(e => e.Employee).WithMany(e => e.User).HasForeignKey(e => e.EmployeeID).IsRequired();
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.EmployeeID).IsUnique();
            });

            modelBuilder.Entity<EmployeeProject>(entity =>
            {
                entity.ToTable("EmployeeProjects");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("EmployeeProjectID");

                entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
                entity.Property(e => e.ProjectID).HasColumnName("ProjectID");
                entity.Property(e => e.AssignmentDate).HasColumnName("assignment_date");
                entity.Property(e => e.AssignmentEndDate).HasColumnName("assignment_end_date");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");

                entity.HasOne(e => e.Employee).WithMany(e => e.EmployeeProject).HasForeignKey(e => e.EmployeeID);
                entity.HasOne(e => e.Project).WithMany(p => p.EmployeeProject).HasForeignKey(e => e.ProjectID);
            });

            modelBuilder.Entity<ActivityType>(entity =>
            {
                entity.ToTable("ActivityTypes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ActivityTypeID");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.ColorCode).HasColumnName("color_code");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.ToTable("Holidays");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("HolidayID");
                entity.Property(e => e.HolidayName).HasColumnName("holiday_name");
                entity.Property(e => e.HolidayDate).HasColumnName("holiday_date");
                entity.Property(e => e.IsRecurring).HasColumnName("is_recurring");
                entity.Property(e => e.HolidayType).HasColumnName("holiday_type");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");
            });

            modelBuilder.Entity<DailyActivity>(entity =>
            {
                entity.ToTable("DailyActivities");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("DailyActivityID");
                entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
                entity.Property(e => e.ProjectID).HasColumnName("Id");
                entity.Property(e => e.ActivityTypeID).HasColumnName("ActivityTypeID");
                entity.Property(e => e.HoursQuantity).HasColumnName("HoursQuantity");
                entity.Property(e => e.ActivityDate).HasColumnName("activity_date");
                entity.Property(e => e.ActivityDescription).HasColumnName("activity_description");
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.IsBillable).HasColumnName("is_billable");
                entity.Property(e => e.ApprovedByID).HasColumnName("approved_by");
                entity.Property(e => e.ApprovalDate).HasColumnName("approval_date");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");

                entity.HasOne(e => e.Employee).WithMany().HasForeignKey(e => e.EmployeeID);
                entity.HasOne(e => e.Project).WithMany().HasForeignKey(e => e.ProjectID);
                entity.HasOne(e => e.ActivityType).WithMany().HasForeignKey(e => e.ActivityTypeID);
                entity.HasOne(e => e.ApprovedByUser).WithMany().HasForeignKey(e => e.ApprovedByID);
            });

            modelBuilder.Entity<Leader>(entity =>
            {
                entity.ToTable("Leaders");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("LeaderID");
                entity.Property(e => e.PersonID).HasColumnName("PersonID");
                entity.Property(e => e.ProjectID).HasColumnName("ProjectID");
                entity.Property(e => e.LeadershipType).HasColumnName("leadership_type");
                entity.Property(e => e.StartDate).HasColumnName("start_date");
                entity.Property(e => e.EndDate).HasColumnName("end_date");
                entity.Property(e => e.Responsibilities).HasColumnName("responsibilities");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");

                entity.HasOne(e => e.Person).WithMany().HasForeignKey(e => e.PersonID);
                entity.HasOne(e => e.Project).WithMany().HasForeignKey(e => e.ProjectID);
            });

            modelBuilder.Entity<PermissionType>(entity =>
            {
                entity.ToTable("PermissionTypes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("PermissionTypeID");
                entity.Property(e => e.TypeCode).HasColumnName("type_code");
                entity.Property(e => e.TypeName).HasColumnName("type_name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.IsPaid).HasColumnName("is_paid");
                entity.Property(e => e.RequiresApproval).HasColumnName("requires_approval");
                entity.Property(e => e.MaxDays).HasColumnName("max_days");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permissions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("PermissionID");
                entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
                entity.Property(e => e.PermissionTypeID).HasColumnName("PermissionTypeID");
                entity.Property(e => e.StartDate).HasColumnName("start_date");
                entity.Property(e => e.EndDate).HasColumnName("end_date");
                entity.Property(e => e.TotalDays).HasColumnName("total_days");
                entity.Property(e => e.TotalHours).HasColumnName("total_hours");
                entity.Property(e => e.IsPaid).HasColumnName("is_paid");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.ApprovalStatus).HasColumnName("approval_status");
                entity.Property(e => e.ApprovedByID).HasColumnName("approved_by");
                entity.Property(e => e.ApprovalDate).HasColumnName("approval_date");
                entity.Property(e => e.Observation).HasColumnName("observation");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");

                entity.HasOne(e => e.Employee).WithMany().HasForeignKey(e => e.EmployeeID);
                entity.HasOne(e => e.PermissionType).WithMany().HasForeignKey(e => e.PermissionTypeID);
                entity.HasOne(e => e.ApprovedBy).WithMany().HasForeignKey(e => e.ApprovedByID);
            });

            modelBuilder.Entity<Historic>(entity =>
            {
                entity.ToTable("Historics");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("HistoricID");
                entity.Property(e => e.DailyActivityID).HasColumnName("DailyActivityID");
                entity.Property(e => e.ChangeType).HasColumnName("change_type");
                entity.Property(e => e.OldStartTime).HasColumnName("old_start_time");
                entity.Property(e => e.OldEndTime).HasColumnName("old_end_time");
                entity.Property(e => e.OldDescription).HasColumnName("old_description");
                entity.Property(e => e.OldHours).HasColumnName("old_hours");
                entity.Property(e => e.NewStartTime).HasColumnName("new_start_time");
                entity.Property(e => e.NewEndTime).HasColumnName("new_end_time");
                entity.Property(e => e.NewDescription).HasColumnName("new_description");
                entity.Property(e => e.NewHours).HasColumnName("new_hours");
                entity.Property(e => e.ChangeReason).HasColumnName("change_reason");
                entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
                entity.Property(e => e.ChangeDate).HasColumnName("change_date");
                entity.Property(e => e.ChangeIP).HasColumnName("change_ip");

                entity.HasOne(e => e.DailyActivity).WithMany().HasForeignKey(e => e.DailyActivityID);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("RoleID");
                entity.Property(e => e.RoleName).HasColumnName("role_name");
                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");

                entity.HasMany(e => e.UserRole)
                      .WithOne(e => e.Role)
                      .HasForeignKey(e => e.RoleID);

                entity.HasMany(e => e.RoleModule)
                      .WithOne(e => e.Role)
                      .HasForeignKey(e => e.RoleID);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("UserRoleID");
                entity.Property(e => e.UserID).HasColumnName("UserID");
                entity.Property(e => e.RoleID).HasColumnName("RoleID");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");

                entity.HasOne(e => e.Role)
                      .WithMany(e => e.UserRole)
                      .HasForeignKey(e => e.RoleID);

                entity.HasOne(e => e.User)
                      .WithMany(e => e.UserRole)
                      .HasForeignKey(e => e.UserID);

                entity.HasIndex(e => new { e.UserID, e.RoleID })
                      .IsUnique()
                      .HasDatabaseName("UQ_UserRoles_UserRole");
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.ToTable("Modules");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ModuleID");
                entity.Property(e => e.ModuleName).HasColumnName("module_name");
                entity.Property(e => e.ModulePath).HasColumnName("module_path");
                entity.Property(e => e.Icon).HasColumnName("icon");
                entity.Property(e => e.DisplayOrder).HasColumnName("display_order");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");

                entity.HasMany(e => e.RoleModules)
                      .WithOne(e => e.Module)
                      .HasForeignKey(e => e.ModuleID);
            });

            modelBuilder.Entity<RoleModule>(entity =>
            {
                entity.ToTable("RoleModules");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("RoleModuleID");
                entity.Property(e => e.RoleID).HasColumnName("RoleID");
                entity.Property(e => e.ModuleID).HasColumnName("ModuleID");
                entity.Property(e => e.CanView).HasColumnName("can_view");
                entity.Property(e => e.CanCreate).HasColumnName("can_create");
                entity.Property(e => e.CanEdit).HasColumnName("can_edit");
                entity.Property(e => e.CanDelete).HasColumnName("can_delete");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreationUser).HasColumnName("creation_user");
                entity.Property(e => e.ModificationUser).HasColumnName("modification_user");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.ModificationDate).HasColumnName("modification_date");
                entity.Property(e => e.CreationIp).HasColumnName("creation_ip");
                entity.Property(e => e.ModificationIp).HasColumnName("modification_ip");

                entity.HasOne(e => e.Role)
                      .WithMany(e => e.RoleModule)
                      .HasForeignKey(e => e.RoleID);

                entity.HasOne(e => e.Module)
                      .WithMany(e => e.RoleModules)
                      .HasForeignKey(e => e.ModuleID);

                entity.HasIndex(e => new { e.RoleID, e.ModuleID })
                      .IsUnique()
                      .HasDatabaseName("UQ_RoleModules_RoleModule");
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Nationality> Nationality { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<IdentificationType> IdentificationTypes { get; set; }
        public DbSet<Position> Positions {  get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ProjectStatus> ProjectStatus { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<DailyActivity> DailyActivities { get; set; }
        public DbSet<Leader> Leaders { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Historic> Historics { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<RoleModule> RoleModules { get; set; }
    }
}
