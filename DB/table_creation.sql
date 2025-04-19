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


INSERT INTO public."question" ( "text", "category", "createdAt", "isDeleted")
VALUES
    ('Disfruto trabajar con herramientas y máquinas', 'Realistic', CURRENT_TIMESTAMP, FALSE),
    ('Me gusta construir cosas con mis manos', 'Realistic', CURRENT_TIMESTAMP, FALSE),
    ('Prefiero actividades que impliquen esfuerzo físico', 'Realistic', CURRENT_TIMESTAMP, FALSE),
    ('Disfruto reparar dispositivos mecánicos', 'Realistic', CURRENT_TIMESTAMP, FALSE),
    ('Soy bueno usando herramientas', 'Realistic', CURRENT_TIMESTAMP, FALSE),
    ('Disfruto resolver problemas complejos', 'Investigative', CURRENT_TIMESTAMP, FALSE),
    ('Me gusta analizar datos e información', 'Investigative', CURRENT_TIMESTAMP, FALSE),
    ('Disfruto la ciencia y los experimentos científicos', 'Investigative', CURRENT_TIMESTAMP, FALSE),
    ('Siento curiosidad por cómo funcionan las cosas', 'Investigative', CURRENT_TIMESTAMP, FALSE),
    ('Disfruto los acertijos intelectuales desafiantes', 'Investigative', CURRENT_TIMESTAMP, FALSE),
    ('Disfruto expresarme de forma creativa', 'Artistic', CURRENT_TIMESTAMP, FALSE),
    ('Me gustan las actividades que permiten la autoexpresión', 'Artistic', CURRENT_TIMESTAMP, FALSE),
    ('Disfruto el arte, la música o la escritura creativa', 'Artistic', CURRENT_TIMESTAMP, FALSE),
    ('Prefiero tareas sin reglas ni estructuras rígidas', 'Artistic', CURRENT_TIMESTAMP, FALSE),
    ('Soy bueno pensando en nuevas ideas', 'Artistic', CURRENT_TIMESTAMP, FALSE),
    ('Disfruto ayudar a otros con sus problemas', 'Social', CURRENT_TIMESTAMP, FALSE),
    ('Me gusta enseñar o capacitar a otros', 'Social', CURRENT_TIMESTAMP, FALSE),
    ('Soy bueno entendiendo los sentimientos de las personas', 'Social', CURRENT_TIMESTAMP, FALSE),
    ('Disfruto trabajar en equipo', 'Social', CURRENT_TIMESTAMP, FALSE),
    ('Me interesan los temas y causas sociales', 'Social', CURRENT_TIMESTAMP, FALSE),
    ('Disfruto persuadir a otros para que hagan las cosas a mi manera', 'Enterprising', CURRENT_TIMESTAMP, FALSE),
    ('Me gusta liderar y dirigir a otros', 'Enterprising', CURRENT_TIMESTAMP, FALSE),
    ('Disfruto iniciar y llevar a cabo proyectos', 'Enterprising', CURRENT_TIMESTAMP, FALSE),
    ('Soy bueno vendiendo cosas o promoviendo ideas', 'Enterprising', CURRENT_TIMESTAMP, FALSE),
    ('Me gusta asumir riesgos para lograr objetivos', 'Enterprising', CURRENT_TIMESTAMP, FALSE),
    ('Disfruto trabajar con números y datos', 'Conventional', CURRENT_TIMESTAMP, FALSE),
    ('Me gusta seguir procedimientos y reglas claras', 'Conventional', CURRENT_TIMESTAMP, FALSE),
    ('Soy bueno organizando y manteniendo registros', 'Conventional', CURRENT_TIMESTAMP, FALSE),
    ('Presto atención a los detalles', 'Conventional', CURRENT_TIMESTAMP, FALSE),
    ('Prefiero tareas estructuradas con instrucciones claras', 'Conventional', CURRENT_TIMESTAMP, FALSE);