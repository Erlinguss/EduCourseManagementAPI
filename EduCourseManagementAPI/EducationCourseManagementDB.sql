DROP DATABASE IF EXISTS EducationCourseManagementDB;
CREATE DATABASE IF NOT EXISTS EducationCourseManagementDB;
USE EducationCourseManagementDB;


DROP TABLE IF EXISTS StudentCourses;
DROP TABLE IF EXISTS Schedules;
DROP TABLE IF EXISTS Students;
DROP TABLE IF EXISTS Courses;
DROP TABLE IF EXISTS Instructors;


CREATE TABLE Courses (
    CourseId INT PRIMARY KEY AUTO_INCREMENT,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Credits INT NOT NULL
);


INSERT INTO Courses (Title, Description, Credits) VALUES 
('Introduction to Programming', 'Basic programming concepts', 3),
('Data Structures', 'In-depth look at data structures', 4),
('Database Systems', 'Introduction to relational databases', 3),
('Web Development', 'Building websites with HTML, CSS, and JavaScript', 3),
('Software Engineering', 'Principles of software design and architecture', 4),
('Machine Learning', 'Basics of machine learning and data science', 3),
('Computer Networks', 'Fundamentals of networking and protocols', 3),
('Artificial Intelligence', 'Intro to AI and its applications', 4),
('Operating Systems', 'Principles of operating systems', 4),
('Cyber Security', 'Foundations of security in computer systems', 3);


CREATE TABLE Students (
    StudentId INT PRIMARY KEY AUTO_INCREMENT,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL
);


INSERT INTO Students (Name, Email) VALUES
('Alice Johnson', 'alice.johnson@dkit.com'),
('Bob Smith', 'bob.smith@dkit.com'),
('Charlie Brown', 'charlie.brown@dkit.com'),
('Dana White', 'dana.white@dkit.com'),
('Eve Black', 'eve.black@dkit.com'),
('Frank Martin', 'frank.martin@dkit.com'),
('Grace Lee', 'grace.lee@dkit.com'),
('Hannah Adams', 'hannah.adams@dkit.com'),
('Ian Curtis', 'ian.curtis@dkit.com'),
('Jane Doe', 'jane.doe@dkit.com');


CREATE TABLE Instructors (
    InstructorId INT PRIMARY KEY AUTO_INCREMENT,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL
);


INSERT INTO Instructors (Name, Email) VALUES
('Dr. Emily Thompson', 'emily.thompson@dkit.com'),
('Prof. Michael Green', 'michael.green@dkit.com'),
('Dr. Laura White', 'laura.white@dkit.com'),
('Prof. Robert King', 'robert.king@dkit.com'),
('Dr. Susan Black', 'susan.black@dkit.com');


CREATE TABLE Schedules (
    ScheduleId INT PRIMARY KEY AUTO_INCREMENT,
    CourseId INT,
    InstructorId INT,
    Date DATETIME NOT NULL,
    TimeSlot NVARCHAR(50) NOT NULL,
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId),
    FOREIGN KEY (InstructorId) REFERENCES Instructors(InstructorId)
);


INSERT INTO Schedules (CourseId, InstructorId, Date, TimeSlot) VALUES
(1, 1, '2024-11-20 09:00:00', '9:00 AM - 10:30 AM'),
(2, 2, '2024-11-21 11:00:00', '11:00 AM - 12:30 PM'),
(3, 1, '2024-11-22 10:00:00', '10:00 AM - 11:30 AM'),
(4, 3, '2024-11-23 09:00:00', '9:00 AM - 10:30 AM'),
(5, 2, '2024-11-24 11:00:00', '11:00 AM - 12:30 PM'),
(6, 4, '2024-11-25 13:00:00', '1:00 PM - 2:30 PM'),
(7, 5, '2024-11-26 14:00:00', '2:00 PM - 3:30 PM'),
(8, 3, '2024-11-27 10:00:00', '10:00 AM - 11:30 AM'),
(9, 4, '2024-11-28 09:00:00', '9:00 AM - 10:30 AM'),
(10, 5, '2024-11-29 13:00:00', '1:00 PM - 2:30 PM');


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
(1, 5),
(2, 3),
(2, 4),
(2, 6),
(3, 1),
(3, 7),
(3, 10),
(4, 8),
(4, 5),
(4, 6),
(5, 1),
(5, 9),
(5, 4),
(6, 3),
(6, 7),
(6, 2),
(7, 8),
(7, 5),
(7, 3),
(8, 10),
(8, 6),
(9, 4),
(9, 5),
(10, 9),
(10, 10),
(10, 2);