using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Floss.Database.Migrations
{
    public partial class Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorizationRoleType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationRoleType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProfId = table.Column<long>(nullable: false),
                    DepartmentName = table.Column<string>(maxLength: 4, nullable: true),
                    CourseCode = table.Column<string>(maxLength: 4, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Duration = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    AssId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<long>(nullable: false),
                    AssignmentName = table.Column<string>(maxLength: 100, nullable: true),
                    DueDate = table.Column<DateTime>(nullable: false),
                    LateDueDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.AssId);
                    table.ForeignKey(
                        name: "FK_Assignments_Courses",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentExempt",
                columns: table => new
                {
                    AssId = table.Column<long>(nullable: false),
                    FilePath = table.Column<string>(maxLength: 200, nullable: true),
                    StrippedFilePath = table.Column<string>(maxLength: 200, nullable: true),
                    FileType = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentExempt", x => x.AssId);
                    table.ForeignKey(
                        name: "FK_AssignmentExempt_Assignment_AssId",
                        column: x => x.AssId,
                        principalTable: "Assignment",
                        principalColumn: "AssId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentSubmission",
                columns: table => new
                {
                    AssId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    FilePath = table.Column<string>(maxLength: 200, nullable: true),
                    StrippedFilePath = table.Column<string>(maxLength: 200, nullable: true),
                    EvalFilePath = table.Column<string>(maxLength: 200, nullable: true),
                    FileType = table.Column<string>(maxLength: 10, nullable: true),
                    SubmittedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSubmission", x => new { x.AssId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AssignmentSubmission_Assignment_AssId",
                        column: x => x.AssId,
                        principalTable: "Assignment",
                        principalColumn: "AssId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(nullable: false),
                    AuthorizationRoleTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_AuthorizationRoleType_AuthorizationRoleTypeId",
                        column: x => x.AuthorizationRoleTypeId,
                        principalTable: "AuthorizationRoleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Domain = table.Column<string>(maxLength: 10, nullable: true),
                    AccountName = table.Column<string>(maxLength: 50, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    FullName = table.Column<string>(maxLength: 100, nullable: true),
                    StudentNumber = table.Column<string>(maxLength: 7, nullable: true),
                    UserRoleId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_UserRole_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    Until = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => new { x.UserId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Supervision",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    CourseId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervision", x => new { x.UserId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_Supervisions_Courses",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Supervisions_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AuthorizationRoleType",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { 1L, "Operations" },
                    { 2L, "System Admin" },
                    { 3L, "Developer" },
                    { 4L, "Group Coordinator" }
                });

            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "Id", "CourseCode", "DepartmentName", "Duration", "ProfId", "Year" },
                values: new object[,]
                {
                    { 1L, "4F00", "COSC", "D3", 1L, 2018 },
                    { 2L, "4P80", "COSC", "D4", 1L, 2018 },
                    { 3L, "1p06", "MATH", "D2", 1L, 2018 }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccountName", "DisplayName", "Domain", "Email", "FullName", "StudentNumber", "UserRoleId" },
                values: new object[,]
                {
                    { 1L, "jp14fg", "Jordan Prada", "brocku.ca", "jp14fg@brocku.ca", "Jordan Prada", null, null },
                    { 2L, "dd14wm", "Dana Darmohray", "brocku.ca", "dd14wm@brocku.ca", "Dana Darmohray", null, null },
                    { 3L, "tn16jv", "ThaiBinh Nguyen", "brocku.ca", "tn16jv@brocku.ca", "ThaiBinh Nguyen", null, null },
                    { 4L, "dp14hx", "Dylan Pavao", "brocku.ca", "dp14hx@brocku.ca", "Dylan Pavao", null, null },
                    { 5L, "tk14rs", "Tyler Kisac", "brocku.ca", "tk14rs@brocku.ca", "Tyler Kisac", null, null },
                    { 6L, "ma15om", "Michael Ahle", "brocku.ca", "ma15om@brocku.ca", "Michael Ahle", null, null }
                });

            migrationBuilder.InsertData(
                table: "Assignment",
                columns: new[] { "AssId", "AssignmentName", "CourseId", "DueDate", "LateDueDate" },
                values: new object[,]
                {
                    { 1L, "First Assignment", 1L, new DateTime(2019, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2L, "Second Assignment", 1L, new DateTime(2019, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Enrollment",
                columns: new[] { "UserId", "CourseId", "Until" },
                values: new object[,]
                {
                    { 1L, 1L, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1L, 2L, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1L, 3L, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4L, 1L, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4L, 2L, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4L, 3L, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "Id", "AuthorizationRoleTypeId", "UserId" },
                values: new object[,]
                {
                    { 1L, 1L, 1L },
                    { 2L, 1L, 2L },
                    { 3L, 1L, 3L },
                    { 4L, 1L, 4L },
                    { 5L, 1L, 5L },
                    { 6L, 1L, 6L }
                });

            migrationBuilder.InsertData(
                table: "AssignmentSubmission",
                columns: new[] { "AssId", "UserId", "EvalFilePath", "FilePath", "FileType", "StrippedFilePath", "SubmittedDate" },
                values: new object[] { 1L, 1L, null, "MyMentalState1/reee.cpp", "cpp", "MyMentalState1/reee.txt", new DateTime(2019, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "AssignmentSubmission",
                columns: new[] { "AssId", "UserId", "EvalFilePath", "FilePath", "FileType", "StrippedFilePath", "SubmittedDate" },
                values: new object[] { 1L, 4L, null, "MyMentalState/reee.java", "java", "MyMentalState2/reee.txt", new DateTime(2019, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "AssignmentSubmission",
                columns: new[] { "AssId", "UserId", "EvalFilePath", "FilePath", "FileType", "StrippedFilePath", "SubmittedDate" },
                values: new object[] { 2L, 1L, null, "MyMentalState/reee.c", "c", "MyMentalState3/reee.txt", new DateTime(2019, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_CourseId",
                table: "Assignment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmission_UserId",
                table: "AssignmentSubmission",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseId",
                table: "Enrollment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Supervision_CourseId",
                table: "Supervision",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserRoleId",
                table: "User",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_AuthorizationRoleTypeId",
                table: "UserRole",
                column: "AuthorizationRoleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentSubmission_User_UserId",
                table: "AssignmentSubmission",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole");

            migrationBuilder.DropTable(
                name: "AssignmentExempt");

            migrationBuilder.DropTable(
                name: "AssignmentSubmission");

            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropTable(
                name: "Supervision");

            migrationBuilder.DropTable(
                name: "Assignment");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "AuthorizationRoleType");
        }
    }
}
