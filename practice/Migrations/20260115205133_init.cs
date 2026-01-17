using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace practice.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Elections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ElectionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ResultsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    cnic = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    VoterIdNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PartyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PartySymbol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Manifesto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Education = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PreviousExperience = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TotalVotes = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ElectionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidates_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Candidates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElectionId = table.Column<int>(type: "int", nullable: false),
                    VoterId = table.Column<int>(type: "int", nullable: false),
                    CandidateId = table.Column<int>(type: "int", nullable: false),
                    VotedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votes_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Votes_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Votes_Users_VoterId",
                        column: x => x.VoterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Elections",
                columns: new[] { "Id", "CreatedAt", "Description", "ElectionType", "EndDate", "IsActive", "ResultsPublished", "StartDate", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed General Election", "General", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "National Assembly 2025", null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ongoing Local Election", "Local", new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Local Council 2026", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "IsVerified", "PasswordHash", "PhoneNumber", "Role", "UpdatedAt", "VoterIdNumber", "cnic" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@votingsystem.com", "System Administrator", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923001234567", "Admin", null, null, null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "candidate1@party.com", "Candidate 1", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923000000001", "Candidate", null, null, "4210100000001" },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "candidate2@party.com", "Candidate 2", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923000000002", "Candidate", null, null, "4210100000002" },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "candidate3@party.com", "Candidate 3", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923000000003", "Candidate", null, null, "4210100000003" },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "candidate4@party.com", "Candidate 4", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923000000004", "Candidate", null, null, "4210100000004" },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "candidate5@party.com", "Candidate 5", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923000000005", "Candidate", null, null, "4210100000005" },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "candidate6@party.com", "Candidate 6", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923000000006", "Candidate", null, null, "4210100000006" },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "candidate7@party.com", "Candidate 7", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923000000007", "Candidate", null, null, "4210100000007" },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "candidate8@party.com", "Candidate 8", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923000000008", "Candidate", null, null, "4210100000008" },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "candidate9@party.com", "Candidate 9", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923000000009", "Candidate", null, null, "4210100000009" },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "candidate10@party.com", "Candidate 10", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923000000010", "Candidate", null, null, "4210100000010" },
                    { 101, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter1@mail.com", "Voter 1", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000001", "Voter", null, "VOTER-1001", "4220100000001" },
                    { 102, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter2@mail.com", "Voter 2", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000002", "Voter", null, "VOTER-1002", "4220100000002" },
                    { 103, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter3@mail.com", "Voter 3", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000003", "Voter", null, "VOTER-1003", "4220100000003" },
                    { 104, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter4@mail.com", "Voter 4", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000004", "Voter", null, "VOTER-1004", "4220100000004" },
                    { 105, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter5@mail.com", "Voter 5", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000005", "Voter", null, "VOTER-1005", "4220100000005" },
                    { 106, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter6@mail.com", "Voter 6", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000006", "Voter", null, "VOTER-1006", "4220100000006" },
                    { 107, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter7@mail.com", "Voter 7", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000007", "Voter", null, "VOTER-1007", "4220100000007" },
                    { 108, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter8@mail.com", "Voter 8", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000008", "Voter", null, "VOTER-1008", "4220100000008" },
                    { 109, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter9@mail.com", "Voter 9", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000009", "Voter", null, "VOTER-1009", "4220100000009" },
                    { 110, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter10@mail.com", "Voter 10", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000010", "Voter", null, "VOTER-1010", "4220100000010" },
                    { 111, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter11@mail.com", "Voter 11", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000011", "Voter", null, "VOTER-1011", "4220100000011" },
                    { 112, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter12@mail.com", "Voter 12", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000012", "Voter", null, "VOTER-1012", "4220100000012" },
                    { 113, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter13@mail.com", "Voter 13", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000013", "Voter", null, "VOTER-1013", "4220100000013" },
                    { 114, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter14@mail.com", "Voter 14", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000014", "Voter", null, "VOTER-1014", "4220100000014" },
                    { 115, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter15@mail.com", "Voter 15", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000015", "Voter", null, "VOTER-1015", "4220100000015" },
                    { 116, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter16@mail.com", "Voter 16", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000016", "Voter", null, "VOTER-1016", "4220100000016" },
                    { 117, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter17@mail.com", "Voter 17", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000017", "Voter", null, "VOTER-1017", "4220100000017" },
                    { 118, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter18@mail.com", "Voter 18", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000018", "Voter", null, "VOTER-1018", "4220100000018" },
                    { 119, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter19@mail.com", "Voter 19", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000019", "Voter", null, "VOTER-1019", "4220100000019" },
                    { 120, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter20@mail.com", "Voter 20", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000020", "Voter", null, "VOTER-1020", "4220100000020" },
                    { 121, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter21@mail.com", "Voter 21", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000021", "Voter", null, "VOTER-1021", "4220100000021" },
                    { 122, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter22@mail.com", "Voter 22", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000022", "Voter", null, "VOTER-1022", "4220100000022" },
                    { 123, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter23@mail.com", "Voter 23", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000023", "Voter", null, "VOTER-1023", "4220100000023" },
                    { 124, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter24@mail.com", "Voter 24", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000024", "Voter", null, "VOTER-1024", "4220100000024" },
                    { 125, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter25@mail.com", "Voter 25", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000025", "Voter", null, "VOTER-1025", "4220100000025" },
                    { 126, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter26@mail.com", "Voter 26", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000026", "Voter", null, "VOTER-1026", "4220100000026" },
                    { 127, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter27@mail.com", "Voter 27", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000027", "Voter", null, "VOTER-1027", "4220100000027" },
                    { 128, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter28@mail.com", "Voter 28", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000028", "Voter", null, "VOTER-1028", "4220100000028" },
                    { 129, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter29@mail.com", "Voter 29", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000029", "Voter", null, "VOTER-1029", "4220100000029" },
                    { 130, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter30@mail.com", "Voter 30", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000030", "Voter", null, "VOTER-1030", "4220100000030" },
                    { 131, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter31@mail.com", "Voter 31", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000031", "Voter", null, "VOTER-1031", "4220100000031" },
                    { 132, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter32@mail.com", "Voter 32", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000032", "Voter", null, "VOTER-1032", "4220100000032" },
                    { 133, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter33@mail.com", "Voter 33", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000033", "Voter", null, "VOTER-1033", "4220100000033" },
                    { 134, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter34@mail.com", "Voter 34", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000034", "Voter", null, "VOTER-1034", "4220100000034" },
                    { 135, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter35@mail.com", "Voter 35", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000035", "Voter", null, "VOTER-1035", "4220100000035" },
                    { 136, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter36@mail.com", "Voter 36", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000036", "Voter", null, "VOTER-1036", "4220100000036" },
                    { 137, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter37@mail.com", "Voter 37", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000037", "Voter", null, "VOTER-1037", "4220100000037" },
                    { 138, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter38@mail.com", "Voter 38", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000038", "Voter", null, "VOTER-1038", "4220100000038" },
                    { 139, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter39@mail.com", "Voter 39", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000039", "Voter", null, "VOTER-1039", "4220100000039" },
                    { 140, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter40@mail.com", "Voter 40", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000040", "Voter", null, "VOTER-1040", "4220100000040" },
                    { 141, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter41@mail.com", "Voter 41", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000041", "Voter", null, "VOTER-1041", "4220100000041" },
                    { 142, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter42@mail.com", "Voter 42", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000042", "Voter", null, "VOTER-1042", "4220100000042" },
                    { 143, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter43@mail.com", "Voter 43", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000043", "Voter", null, "VOTER-1043", "4220100000043" },
                    { 144, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter44@mail.com", "Voter 44", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000044", "Voter", null, "VOTER-1044", "4220100000044" },
                    { 145, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter45@mail.com", "Voter 45", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000045", "Voter", null, "VOTER-1045", "4220100000045" },
                    { 146, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter46@mail.com", "Voter 46", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000046", "Voter", null, "VOTER-1046", "4220100000046" },
                    { 147, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter47@mail.com", "Voter 47", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000047", "Voter", null, "VOTER-1047", "4220100000047" },
                    { 148, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter48@mail.com", "Voter 48", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000048", "Voter", null, "VOTER-1048", "4220100000048" },
                    { 149, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter49@mail.com", "Voter 49", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000049", "Voter", null, "VOTER-1049", "4220100000049" },
                    { 150, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter50@mail.com", "Voter 50", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000050", "Voter", null, "VOTER-1050", "4220100000050" },
                    { 151, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter51@mail.com", "Voter 51", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000051", "Voter", null, "VOTER-1051", "4220100000051" },
                    { 152, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter52@mail.com", "Voter 52", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000052", "Voter", null, "VOTER-1052", "4220100000052" },
                    { 153, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter53@mail.com", "Voter 53", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000053", "Voter", null, "VOTER-1053", "4220100000053" },
                    { 154, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter54@mail.com", "Voter 54", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000054", "Voter", null, "VOTER-1054", "4220100000054" },
                    { 155, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter55@mail.com", "Voter 55", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000055", "Voter", null, "VOTER-1055", "4220100000055" },
                    { 156, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter56@mail.com", "Voter 56", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000056", "Voter", null, "VOTER-1056", "4220100000056" },
                    { 157, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter57@mail.com", "Voter 57", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000057", "Voter", null, "VOTER-1057", "4220100000057" },
                    { 158, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter58@mail.com", "Voter 58", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000058", "Voter", null, "VOTER-1058", "4220100000058" },
                    { 159, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter59@mail.com", "Voter 59", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000059", "Voter", null, "VOTER-1059", "4220100000059" },
                    { 160, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter60@mail.com", "Voter 60", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000060", "Voter", null, "VOTER-1060", "4220100000060" },
                    { 161, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter61@mail.com", "Voter 61", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000061", "Voter", null, "VOTER-1061", "4220100000061" },
                    { 162, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter62@mail.com", "Voter 62", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000062", "Voter", null, "VOTER-1062", "4220100000062" },
                    { 163, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter63@mail.com", "Voter 63", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000063", "Voter", null, "VOTER-1063", "4220100000063" },
                    { 164, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter64@mail.com", "Voter 64", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000064", "Voter", null, "VOTER-1064", "4220100000064" },
                    { 165, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter65@mail.com", "Voter 65", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000065", "Voter", null, "VOTER-1065", "4220100000065" },
                    { 166, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter66@mail.com", "Voter 66", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000066", "Voter", null, "VOTER-1066", "4220100000066" },
                    { 167, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter67@mail.com", "Voter 67", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000067", "Voter", null, "VOTER-1067", "4220100000067" },
                    { 168, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter68@mail.com", "Voter 68", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000068", "Voter", null, "VOTER-1068", "4220100000068" },
                    { 169, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter69@mail.com", "Voter 69", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000069", "Voter", null, "VOTER-1069", "4220100000069" },
                    { 170, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter70@mail.com", "Voter 70", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000070", "Voter", null, "VOTER-1070", "4220100000070" },
                    { 171, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter71@mail.com", "Voter 71", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000071", "Voter", null, "VOTER-1071", "4220100000071" },
                    { 172, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter72@mail.com", "Voter 72", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000072", "Voter", null, "VOTER-1072", "4220100000072" },
                    { 173, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter73@mail.com", "Voter 73", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000073", "Voter", null, "VOTER-1073", "4220100000073" },
                    { 174, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter74@mail.com", "Voter 74", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000074", "Voter", null, "VOTER-1074", "4220100000074" },
                    { 175, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter75@mail.com", "Voter 75", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000075", "Voter", null, "VOTER-1075", "4220100000075" },
                    { 176, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter76@mail.com", "Voter 76", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000076", "Voter", null, "VOTER-1076", "4220100000076" },
                    { 177, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter77@mail.com", "Voter 77", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000077", "Voter", null, "VOTER-1077", "4220100000077" },
                    { 178, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter78@mail.com", "Voter 78", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000078", "Voter", null, "VOTER-1078", "4220100000078" },
                    { 179, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter79@mail.com", "Voter 79", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000079", "Voter", null, "VOTER-1079", "4220100000079" },
                    { 180, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter80@mail.com", "Voter 80", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000080", "Voter", null, "VOTER-1080", "4220100000080" },
                    { 181, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter81@mail.com", "Voter 81", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000081", "Voter", null, "VOTER-1081", "4220100000081" },
                    { 182, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter82@mail.com", "Voter 82", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000082", "Voter", null, "VOTER-1082", "4220100000082" },
                    { 183, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter83@mail.com", "Voter 83", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000083", "Voter", null, "VOTER-1083", "4220100000083" },
                    { 184, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter84@mail.com", "Voter 84", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000084", "Voter", null, "VOTER-1084", "4220100000084" },
                    { 185, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter85@mail.com", "Voter 85", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000085", "Voter", null, "VOTER-1085", "4220100000085" },
                    { 186, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter86@mail.com", "Voter 86", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000086", "Voter", null, "VOTER-1086", "4220100000086" },
                    { 187, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter87@mail.com", "Voter 87", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000087", "Voter", null, "VOTER-1087", "4220100000087" },
                    { 188, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter88@mail.com", "Voter 88", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000088", "Voter", null, "VOTER-1088", "4220100000088" },
                    { 189, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter89@mail.com", "Voter 89", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000089", "Voter", null, "VOTER-1089", "4220100000089" },
                    { 190, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter90@mail.com", "Voter 90", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000090", "Voter", null, "VOTER-1090", "4220100000090" },
                    { 191, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter91@mail.com", "Voter 91", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000091", "Voter", null, "VOTER-1091", "4220100000091" },
                    { 192, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter92@mail.com", "Voter 92", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000092", "Voter", null, "VOTER-1092", "4220100000092" },
                    { 193, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter93@mail.com", "Voter 93", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000093", "Voter", null, "VOTER-1093", "4220100000093" },
                    { 194, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter94@mail.com", "Voter 94", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000094", "Voter", null, "VOTER-1094", "4220100000094" },
                    { 195, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter95@mail.com", "Voter 95", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000095", "Voter", null, "VOTER-1095", "4220100000095" },
                    { 196, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter96@mail.com", "Voter 96", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000096", "Voter", null, "VOTER-1096", "4220100000096" },
                    { 197, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter97@mail.com", "Voter 97", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000097", "Voter", null, "VOTER-1097", "4220100000097" },
                    { 198, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter98@mail.com", "Voter 98", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000098", "Voter", null, "VOTER-1098", "4220100000098" },
                    { 199, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter99@mail.com", "Voter 99", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000099", "Voter", null, "VOTER-1099", "4220100000099" },
                    { 200, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter100@mail.com", "Voter 100", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000100", "Voter", null, "VOTER-1100", "4220100000100" },
                    { 201, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter101@mail.com", "Voter 101", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000101", "Voter", null, "VOTER-1101", "4220100000101" },
                    { 202, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter102@mail.com", "Voter 102", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000102", "Voter", null, "VOTER-1102", "4220100000102" },
                    { 203, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter103@mail.com", "Voter 103", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000103", "Voter", null, "VOTER-1103", "4220100000103" },
                    { 204, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter104@mail.com", "Voter 104", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000104", "Voter", null, "VOTER-1104", "4220100000104" },
                    { 205, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter105@mail.com", "Voter 105", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000105", "Voter", null, "VOTER-1105", "4220100000105" },
                    { 206, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter106@mail.com", "Voter 106", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000106", "Voter", null, "VOTER-1106", "4220100000106" },
                    { 207, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter107@mail.com", "Voter 107", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000107", "Voter", null, "VOTER-1107", "4220100000107" },
                    { 208, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter108@mail.com", "Voter 108", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000108", "Voter", null, "VOTER-1108", "4220100000108" },
                    { 209, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter109@mail.com", "Voter 109", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000109", "Voter", null, "VOTER-1109", "4220100000109" },
                    { 210, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter110@mail.com", "Voter 110", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000110", "Voter", null, "VOTER-1110", "4220100000110" },
                    { 211, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter111@mail.com", "Voter 111", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000111", "Voter", null, "VOTER-1111", "4220100000111" },
                    { 212, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter112@mail.com", "Voter 112", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000112", "Voter", null, "VOTER-1112", "4220100000112" },
                    { 213, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter113@mail.com", "Voter 113", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000113", "Voter", null, "VOTER-1113", "4220100000113" },
                    { 214, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter114@mail.com", "Voter 114", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000114", "Voter", null, "VOTER-1114", "4220100000114" },
                    { 215, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter115@mail.com", "Voter 115", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000115", "Voter", null, "VOTER-1115", "4220100000115" },
                    { 216, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter116@mail.com", "Voter 116", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000116", "Voter", null, "VOTER-1116", "4220100000116" },
                    { 217, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter117@mail.com", "Voter 117", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000117", "Voter", null, "VOTER-1117", "4220100000117" },
                    { 218, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter118@mail.com", "Voter 118", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000118", "Voter", null, "VOTER-1118", "4220100000118" },
                    { 219, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter119@mail.com", "Voter 119", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000119", "Voter", null, "VOTER-1119", "4220100000119" },
                    { 220, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter120@mail.com", "Voter 120", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000120", "Voter", null, "VOTER-1120", "4220100000120" },
                    { 221, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter121@mail.com", "Voter 121", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000121", "Voter", null, "VOTER-1121", "4220100000121" },
                    { 222, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter122@mail.com", "Voter 122", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000122", "Voter", null, "VOTER-1122", "4220100000122" },
                    { 223, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter123@mail.com", "Voter 123", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000123", "Voter", null, "VOTER-1123", "4220100000123" },
                    { 224, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter124@mail.com", "Voter 124", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000124", "Voter", null, "VOTER-1124", "4220100000124" },
                    { 225, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter125@mail.com", "Voter 125", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000125", "Voter", null, "VOTER-1125", "4220100000125" },
                    { 226, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter126@mail.com", "Voter 126", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000126", "Voter", null, "VOTER-1126", "4220100000126" },
                    { 227, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter127@mail.com", "Voter 127", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000127", "Voter", null, "VOTER-1127", "4220100000127" },
                    { 228, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter128@mail.com", "Voter 128", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000128", "Voter", null, "VOTER-1128", "4220100000128" },
                    { 229, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter129@mail.com", "Voter 129", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000129", "Voter", null, "VOTER-1129", "4220100000129" },
                    { 230, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter130@mail.com", "Voter 130", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000130", "Voter", null, "VOTER-1130", "4220100000130" },
                    { 231, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter131@mail.com", "Voter 131", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000131", "Voter", null, "VOTER-1131", "4220100000131" },
                    { 232, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter132@mail.com", "Voter 132", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000132", "Voter", null, "VOTER-1132", "4220100000132" },
                    { 233, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter133@mail.com", "Voter 133", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000133", "Voter", null, "VOTER-1133", "4220100000133" },
                    { 234, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter134@mail.com", "Voter 134", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000134", "Voter", null, "VOTER-1134", "4220100000134" },
                    { 235, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter135@mail.com", "Voter 135", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000135", "Voter", null, "VOTER-1135", "4220100000135" },
                    { 236, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter136@mail.com", "Voter 136", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000136", "Voter", null, "VOTER-1136", "4220100000136" },
                    { 237, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter137@mail.com", "Voter 137", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000137", "Voter", null, "VOTER-1137", "4220100000137" },
                    { 238, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter138@mail.com", "Voter 138", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000138", "Voter", null, "VOTER-1138", "4220100000138" },
                    { 239, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter139@mail.com", "Voter 139", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000139", "Voter", null, "VOTER-1139", "4220100000139" },
                    { 240, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter140@mail.com", "Voter 140", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000140", "Voter", null, "VOTER-1140", "4220100000140" },
                    { 241, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter141@mail.com", "Voter 141", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000141", "Voter", null, "VOTER-1141", "4220100000141" },
                    { 242, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter142@mail.com", "Voter 142", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000142", "Voter", null, "VOTER-1142", "4220100000142" },
                    { 243, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter143@mail.com", "Voter 143", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000143", "Voter", null, "VOTER-1143", "4220100000143" },
                    { 244, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter144@mail.com", "Voter 144", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000144", "Voter", null, "VOTER-1144", "4220100000144" },
                    { 245, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter145@mail.com", "Voter 145", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000145", "Voter", null, "VOTER-1145", "4220100000145" },
                    { 246, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter146@mail.com", "Voter 146", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000146", "Voter", null, "VOTER-1146", "4220100000146" },
                    { 247, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter147@mail.com", "Voter 147", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000147", "Voter", null, "VOTER-1147", "4220100000147" },
                    { 248, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter148@mail.com", "Voter 148", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000148", "Voter", null, "VOTER-1148", "4220100000148" },
                    { 249, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter149@mail.com", "Voter 149", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000149", "Voter", null, "VOTER-1149", "4220100000149" },
                    { 250, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter150@mail.com", "Voter 150", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000150", "Voter", null, "VOTER-1150", "4220100000150" },
                    { 251, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter151@mail.com", "Voter 151", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000151", "Voter", null, "VOTER-1151", "4220100000151" },
                    { 252, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter152@mail.com", "Voter 152", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000152", "Voter", null, "VOTER-1152", "4220100000152" },
                    { 253, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter153@mail.com", "Voter 153", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000153", "Voter", null, "VOTER-1153", "4220100000153" },
                    { 254, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter154@mail.com", "Voter 154", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000154", "Voter", null, "VOTER-1154", "4220100000154" },
                    { 255, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter155@mail.com", "Voter 155", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000155", "Voter", null, "VOTER-1155", "4220100000155" },
                    { 256, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter156@mail.com", "Voter 156", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000156", "Voter", null, "VOTER-1156", "4220100000156" },
                    { 257, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter157@mail.com", "Voter 157", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000157", "Voter", null, "VOTER-1157", "4220100000157" },
                    { 258, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter158@mail.com", "Voter 158", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000158", "Voter", null, "VOTER-1158", "4220100000158" },
                    { 259, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter159@mail.com", "Voter 159", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000159", "Voter", null, "VOTER-1159", "4220100000159" },
                    { 260, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter160@mail.com", "Voter 160", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000160", "Voter", null, "VOTER-1160", "4220100000160" },
                    { 261, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter161@mail.com", "Voter 161", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000161", "Voter", null, "VOTER-1161", "4220100000161" },
                    { 262, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter162@mail.com", "Voter 162", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000162", "Voter", null, "VOTER-1162", "4220100000162" },
                    { 263, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter163@mail.com", "Voter 163", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000163", "Voter", null, "VOTER-1163", "4220100000163" },
                    { 264, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter164@mail.com", "Voter 164", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000164", "Voter", null, "VOTER-1164", "4220100000164" },
                    { 265, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter165@mail.com", "Voter 165", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000165", "Voter", null, "VOTER-1165", "4220100000165" },
                    { 266, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter166@mail.com", "Voter 166", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000166", "Voter", null, "VOTER-1166", "4220100000166" },
                    { 267, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter167@mail.com", "Voter 167", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000167", "Voter", null, "VOTER-1167", "4220100000167" },
                    { 268, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter168@mail.com", "Voter 168", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000168", "Voter", null, "VOTER-1168", "4220100000168" },
                    { 269, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter169@mail.com", "Voter 169", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000169", "Voter", null, "VOTER-1169", "4220100000169" },
                    { 270, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter170@mail.com", "Voter 170", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000170", "Voter", null, "VOTER-1170", "4220100000170" },
                    { 271, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter171@mail.com", "Voter 171", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000171", "Voter", null, "VOTER-1171", "4220100000171" },
                    { 272, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter172@mail.com", "Voter 172", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000172", "Voter", null, "VOTER-1172", "4220100000172" },
                    { 273, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter173@mail.com", "Voter 173", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000173", "Voter", null, "VOTER-1173", "4220100000173" },
                    { 274, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter174@mail.com", "Voter 174", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000174", "Voter", null, "VOTER-1174", "4220100000174" },
                    { 275, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter175@mail.com", "Voter 175", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000175", "Voter", null, "VOTER-1175", "4220100000175" },
                    { 276, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter176@mail.com", "Voter 176", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000176", "Voter", null, "VOTER-1176", "4220100000176" },
                    { 277, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter177@mail.com", "Voter 177", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000177", "Voter", null, "VOTER-1177", "4220100000177" },
                    { 278, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter178@mail.com", "Voter 178", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000178", "Voter", null, "VOTER-1178", "4220100000178" },
                    { 279, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter179@mail.com", "Voter 179", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000179", "Voter", null, "VOTER-1179", "4220100000179" },
                    { 280, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter180@mail.com", "Voter 180", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000180", "Voter", null, "VOTER-1180", "4220100000180" },
                    { 281, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter181@mail.com", "Voter 181", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000181", "Voter", null, "VOTER-1181", "4220100000181" },
                    { 282, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter182@mail.com", "Voter 182", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000182", "Voter", null, "VOTER-1182", "4220100000182" },
                    { 283, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter183@mail.com", "Voter 183", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000183", "Voter", null, "VOTER-1183", "4220100000183" },
                    { 284, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter184@mail.com", "Voter 184", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000184", "Voter", null, "VOTER-1184", "4220100000184" },
                    { 285, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter185@mail.com", "Voter 185", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000185", "Voter", null, "VOTER-1185", "4220100000185" },
                    { 286, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter186@mail.com", "Voter 186", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000186", "Voter", null, "VOTER-1186", "4220100000186" },
                    { 287, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter187@mail.com", "Voter 187", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000187", "Voter", null, "VOTER-1187", "4220100000187" },
                    { 288, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter188@mail.com", "Voter 188", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000188", "Voter", null, "VOTER-1188", "4220100000188" },
                    { 289, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter189@mail.com", "Voter 189", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000189", "Voter", null, "VOTER-1189", "4220100000189" },
                    { 290, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter190@mail.com", "Voter 190", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000190", "Voter", null, "VOTER-1190", "4220100000190" },
                    { 291, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter191@mail.com", "Voter 191", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000191", "Voter", null, "VOTER-1191", "4220100000191" },
                    { 292, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter192@mail.com", "Voter 192", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000192", "Voter", null, "VOTER-1192", "4220100000192" },
                    { 293, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter193@mail.com", "Voter 193", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000193", "Voter", null, "VOTER-1193", "4220100000193" },
                    { 294, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter194@mail.com", "Voter 194", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000194", "Voter", null, "VOTER-1194", "4220100000194" },
                    { 295, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter195@mail.com", "Voter 195", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000195", "Voter", null, "VOTER-1195", "4220100000195" },
                    { 296, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter196@mail.com", "Voter 196", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000196", "Voter", null, "VOTER-1196", "4220100000196" },
                    { 297, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter197@mail.com", "Voter 197", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000197", "Voter", null, "VOTER-1197", "4220100000197" },
                    { 298, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter198@mail.com", "Voter 198", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000198", "Voter", null, "VOTER-1198", "4220100000198" },
                    { 299, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter199@mail.com", "Voter 199", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000199", "Voter", null, "VOTER-1199", "4220100000199" },
                    { 300, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "voter200@mail.com", "Voter 200", true, true, "$2a$11$seX7Ayuffe0Hdpmt0v2gt.CWydeU.5JDXsKyHadXWPxNFvR8JFzmu", "923110000200", "Voter", null, "VOTER-1200", "4220100000200" }
                });

            migrationBuilder.InsertData(
                table: "Candidates",
                columns: new[] { "Id", "Biography", "Education", "ElectionId", "IsApproved", "Manifesto", "PartyName", "PartySymbol", "PreviousExperience", "ProfileImageUrl", "RegisteredAt", "TotalVotes", "UserId" },
                values: new object[,]
                {
                    { 1, "Political experience: 2 years", null, 1, true, "Manifesto of candidate 1", "Republic Alliance", "Lion", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 11 },
                    { 2, "Political experience: 4 years", null, 1, true, "Manifesto of candidate 2", "Democratic Party", "Eagle", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 12 },
                    { 3, "Political experience: 6 years", null, 1, true, "Manifesto of candidate 3", "Republic Alliance", "Lion", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 13 },
                    { 4, "Political experience: 8 years", null, 1, true, "Manifesto of candidate 4", "Democratic Party", "Eagle", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 14 },
                    { 5, "Political experience: 10 years", null, 1, true, "Manifesto of candidate 5", "Republic Alliance", "Lion", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 15 },
                    { 6, "Political experience: 12 years", null, 1, true, "Manifesto of candidate 6", "Democratic Party", "Eagle", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 16 },
                    { 7, "Political experience: 14 years", null, 1, true, "Manifesto of candidate 7", "Republic Alliance", "Lion", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 17 },
                    { 8, "Political experience: 16 years", null, 1, true, "Manifesto of candidate 8", "Democratic Party", "Eagle", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 18 },
                    { 9, "Political experience: 18 years", null, 1, true, "Manifesto of candidate 9", "Republic Alliance", "Lion", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 19 },
                    { 10, "Political experience: 20 years", null, 1, true, "Manifesto of candidate 10", "Democratic Party", "Eagle", null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 20 }
                });

            migrationBuilder.InsertData(
                table: "Votes",
                columns: new[] { "Id", "CandidateId", "ElectionId", "IpAddress", "IsVerified", "VotedAt", "VoterId" },
                values: new object[,]
                {
                    { 1, 2, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 2, 3, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 3, 4, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 4, 5, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 5, 1, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 6, 2, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 7, 3, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 107 },
                    { 8, 4, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 108 },
                    { 9, 5, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 109 },
                    { 10, 1, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 110 },
                    { 11, 2, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 111 },
                    { 12, 3, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 112 },
                    { 13, 4, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 113 },
                    { 14, 5, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 114 },
                    { 15, 1, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 115 },
                    { 16, 2, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 116 },
                    { 17, 3, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 117 },
                    { 18, 4, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 118 },
                    { 19, 5, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 119 },
                    { 20, 1, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 120 },
                    { 21, 2, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 121 },
                    { 22, 3, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 122 },
                    { 23, 4, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 123 },
                    { 24, 5, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 124 },
                    { 25, 1, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 125 },
                    { 26, 2, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 126 },
                    { 27, 3, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 127 },
                    { 28, 4, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 128 },
                    { 29, 5, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 129 },
                    { 30, 1, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 130 },
                    { 31, 2, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 131 },
                    { 32, 3, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 132 },
                    { 33, 4, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 133 },
                    { 34, 5, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 134 },
                    { 35, 1, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 135 },
                    { 36, 2, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 136 },
                    { 37, 3, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 137 },
                    { 38, 4, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 138 },
                    { 39, 5, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 139 },
                    { 40, 1, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 140 },
                    { 41, 2, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 141 },
                    { 42, 3, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 142 },
                    { 43, 4, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 143 },
                    { 44, 5, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 144 },
                    { 45, 1, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 145 },
                    { 46, 2, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 146 },
                    { 47, 3, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 147 },
                    { 48, 4, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 148 },
                    { 49, 5, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 149 },
                    { 50, 1, 1, null, true, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 150 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_ElectionId",
                table: "Candidates",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_UserId",
                table: "Candidates",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_VoterIdNumber",
                table: "Users",
                column: "VoterIdNumber",
                unique: true,
                filter: "[VoterIdNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_CandidateId",
                table: "Votes",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_ElectionId_VoterId",
                table: "Votes",
                columns: new[] { "ElectionId", "VoterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_VoterId",
                table: "Votes",
                column: "VoterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Elections");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
