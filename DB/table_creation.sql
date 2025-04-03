CREATE TABLE Region (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL
);

CREATE TABLE Schedule (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

CREATE TABLE KnowledgeArea (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL
);

CREATE TABLE Institution (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Type VARCHAR(100) NOT NULL,
    RegionId INT REFERENCES Region(Id)
);

CREATE TABLE Career (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    InstitutionId INT REFERENCES Institution(Id),
    KnowledgeAreaId INT REFERENCES KnowledgeArea(Id),
    ScheduleId INT REFERENCES Schedule(Id)
);

CREATE TABLE EmploymentIncome (
    Id SERIAL PRIMARY KEY,
    CareerId INT REFERENCES Career(Id),
    EmploymentRate DECIMAL(5,2) NOT NULL,
    AverageIncome INT NOT NULL,
    MedianIncome INT NOT NULL
);

CREATE TABLE Statistics (
    Id SERIAL PRIMARY KEY,
    CareerId INT REFERENCES Career(Id),
    Enrollment INT NOT NULL,
    GraduationRate DECIMAL(5,2) NOT NULL,
    DropoutRate DECIMAL(5,2) NOT NULL
);

CREATE TABLE WeightingCareer (
    Id SERIAL PRIMARY KEY,
    CareerId INT REFERENCES Career(Id),
    TestType VARCHAR(100) NOT NULL,
    Weight DECIMAL(5,2) NOT NULL
);     

CREATE TABLE Student (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Answers VARCHAR(500) NOT NULL
);

CREATE TABLE WeightingStudent (
    Id SERIAL PRIMARY KEY,
    StudentId INT REFERENCES Student(Id),
    TestType VARCHAR(100) NOT NULL,
    Weight DECIMAL(5,2) NOT NULL
);

CREATE TABLE Email (
    Id SERIAL PRIMARY KEY,
    Type VARCHAR(255) NOT NULL,
    StudentId INT REFERENCES Student(Id),
    DeliveryDate Date NOT NULL,
    IsDelivered boolean NOT NULL
);