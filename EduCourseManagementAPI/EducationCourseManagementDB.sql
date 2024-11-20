/*DROP DATABASE IF EXISTS EducationCourseManagementDB;
CREATE DATABASE IF NOT EXISTS EducationCourseManagementDB;
USE EducationCourseManagementDB;

-- Drop existing tables
DROP TABLE IF EXISTS StudentCourses;
DROP TABLE IF EXISTS Schedules;
DROP TABLE IF EXISTS Students;
DROP TABLE IF EXISTS Courses;
DROP TABLE IF EXISTS Instructors;
DROP TABLE IF EXISTS Rooms;
DROP TABLE IF EXISTS Users;

-- Create Users Table
CREATE TABLE Users (
    UserId INT PRIMARY KEY AUTO_INCREMENT,
    Username NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL -- Admin, Instructor, Student
);

-- Insert Users Data
INSERT INTO Users (Username, Password, Role) VALUES
('PeterAdmin', 'admindkit', 'Admin'),
('JohnInstructor', 'instructordkit', 'Instructor'),
('EmilyInstructor', 'emily123', 'Instructor'),
('MichaelInstructor', 'michael123', 'Instructor'),
('LauraInstructor', 'laura123', 'Instructor'),
('DavidStudent', 'studentdkit', 'Student'),
('AliceStudent', 'alice123', 'Student'),
('BobStudent', 'bob123', 'Student');

-- Create Rooms Table
CREATE TABLE Rooms (
    RoomId INT PRIMARY KEY AUTO_INCREMENT,
    RoomName NVARCHAR(100) NOT NULL,
    Capacity INT NOT NULL
);

-- Insert Rooms Data
INSERT INTO Rooms (RoomName, Capacity) VALUES
('Room A', 30),
('Room B', 50),
('Room C', 25),
('Room D', 40),
('Room E', 35);

-- Create Courses Table
CREATE TABLE Courses (
    CourseId INT PRIMARY KEY AUTO_INCREMENT,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Credits INT NOT NULL
);

-- Insert Courses Data
INSERT INTO Courses (Title, Description, Credits) VALUES 
('Introduction to Programming', 'Basic programming concepts', 3),
('Data Structures', 'In-depth look at data structures', 4),
('Database Systems', 'Introduction to relational databases', 3),
('Web Development', 'Building websites with HTML, CSS, and JavaScript', 3);

-- Create Students Table
CREATE TABLE Students (
    StudentId INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT UNIQUE, -- Link to Users table
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- Insert Students Data
INSERT INTO Students (UserId, Name, Email) VALUES
(6, 'David Student', 'david.student@dkit.com'),
(7, 'Alice Student', 'alice.student@dkit.com'),
(8, 'Bob Student', 'bob.student@dkit.com');

-- Create Instructors Table
CREATE TABLE Instructors (
    InstructorId INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT UNIQUE, -- Link to Users table
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- Insert Instructors Data
INSERT INTO Instructors (UserId, Name, Email) VALUES
(2, 'John Instructor', 'john.instructor@dkit.com'),
(3, 'Emily Instructor', 'emily.instructor@dkit.com'),
(4, 'Michael Instructor', 'michael.instructor@dkit.com'),
(5, 'Laura Instructor', 'laura.instructor@dkit.com');

-- Create Schedules Table
CREATE TABLE Schedules (
    ScheduleId INT PRIMARY KEY AUTO_INCREMENT,
    CourseId INT,
    InstructorId INT,
    RoomId INT,
    Date DATETIME NOT NULL,
    TimeSlot NVARCHAR(50) NOT NULL,
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId),
    FOREIGN KEY (InstructorId) REFERENCES Instructors(InstructorId),
    FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId)
);

-- Insert Schedules Data
INSERT INTO Schedules (CourseId, InstructorId, RoomId, Date, TimeSlot) VALUES
(1, 1, 1, '2024-11-20 09:00:00', '9:00 AM - 10:30 AM'),
(2, 2, 2, '2024-11-21 11:00:00', '11:00 AM - 12:30 PM'),
(3, 3, 3, '2024-11-22 10:00:00', '10:00 AM - 11:30 AM'),
(4, 4, 4, '2024-11-23 09:00:00', '9:00 AM - 10:30 AM');


CREATE TABLE StudentCourses (
    StudentId INT,
    CourseId INT,
    PRIMARY KEY (StudentId, CourseId),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId)
);

INSERT INTO StudentCourses (StudentId, CourseId) VALUES
(1, 1), 
(1, 2), 
(1, 3), 
(2, 3), 
(2, 4), 
(3, 1), 
(3, 2), 
(3, 4);

*/


