CREATE TABLE question (
                          id INT PRIMARY KEY IDENTITY,
                          text VARCHAR(255) NOT NULL,
                          category VARCHAR(255) NOT NULL,
                          isDeleted BOOLEAN NOT NULL
);

INSERT INTO question ( text, category, isDeleted)
VALUES
    ('Disfruto trabajar con herramientas y máquinas', 'Realistic', FALSE),
    ('Me gusta construir cosas con mis manos', 'Realistic', FALSE),
    ('Prefiero actividades que impliquen esfuerzo físico', 'Realistic', FALSE),
    ('Disfruto reparar dispositivos mecánicos', 'Realistic', FALSE),
    ('Soy bueno usando herramientas', 'Realistic', FALSE),
    ('Disfruto resolver problemas complejos', 'Investigative', FALSE),
    ('Me gusta analizar datos e información', 'Investigative', FALSE),
    ('Disfruto la ciencia y los experimentos científicos', 'Investigative', FALSE),
    ('Siento curiosidad por cómo funcionan las cosas', 'Investigative', FALSE),
    ('Disfruto los acertijos intelectuales desafiantes', 'Investigative', FALSE),
    ('Disfruto expresarme de forma creativa', 'Artistic', FALSE),
    ('Me gustan las actividades que permiten la autoexpresión', 'Artistic', FALSE),
    ('Disfruto el arte, la música o la escritura creativa', 'Artistic', FALSE),
    ('Prefiero tareas sin reglas ni estructuras rígidas', 'Artistic', FALSE),
    ('Soy bueno pensando en nuevas ideas', 'Artistic', FALSE),
    ('Disfruto ayudar a otros con sus problemas', 'Social', FALSE),
    ('Me gusta enseñar o capacitar a otros', 'Social', FALSE),
    ('Soy bueno entendiendo los sentimientos de las personas', 'Social', FALSE),
    ('Disfruto trabajar en equipo', 'Social', FALSE),
    ('Me interesan los temas y causas sociales', 'Social', FALSE),
    ('Disfruto persuadir a otros para que hagan las cosas a mi manera', 'Enterprising', FALSE),
    ('Me gusta liderar y dirigir a otros', 'Enterprising', FALSE),
    ('Disfruto iniciar y llevar a cabo proyectos', 'Enterprising', FALSE),
    ('Soy bueno vendiendo cosas o promoviendo ideas', 'Enterprising', FALSE),
    ('Me gusta asumir riesgos para lograr objetivos', 'Enterprising', FALSE),
    ('Disfruto trabajar con números y datos', 'Conventional', FALSE),
    ('Me gusta seguir procedimientos y reglas claras', 'Conventional', FALSE),
    ('Soy bueno organizando y manteniendo registros', 'Conventional', FALSE),
    ('Presto atención a los detalles', 'Conventional', FALSE),
    ('Prefiero tareas estructuradas con instrucciones claras', 'Conventional', FALSE);


CREATE TABLE student (
                         id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                         name VARCHAR(255) NOT NULL,
                         email VARCHAR(255) NOT NULL,
                         answers VARCHAR(500) NOT NULL
);

CREATE TABLE email (
                       id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                       type VARCHAR(255) NOT NULL,
                       studentId INT REFERENCES student(id),
                       deliveryDate Date NOT NULL,
                       isDelivered bit NOT NULL
);


CREATE TABLE institutionType (
                                 id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                                 name VARCHAR(250) NOT NULL,
                                 isDeleted BOOLEAN NOT NULL
);

INSERT INTO institutiontype (name, isdeleted)
VALUES
    ('Centros de Formación Técnica', FALSE),
    ('Universidades', FALSE),
    ('Institutos Profesionales', FALSE);


CREATE TABLE acreditationType (
                                  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                                  name VARCHAR(250) NOT NULL,
                                  isDeleted BOOLEAN NOT NULL
);

INSERT INTO acreditationType ( name, isDeleted)
VALUES
    ( 'No Acreditada', FALSE),
    ( 'Bajo tutela', FALSE),
    ( 'Acreditada', FALSE),
    ( 'Acreditación Extendida', FALSE);

CREATE TABLE institution (
                             id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                             isDeleted BOOLEAN NOT NULL,
                             code VARCHAR(250) NULL, -- Código institución
                             name VARCHAR(250) NOT NULL, -- Nombre institución
                             institutionTypeId INT REFERENCES institutionType(id) -- Tipo de institución 
);

CREATE TABLE institutionDetails(
                                   id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                                   isDeleted BOOLEAN NOT NULL,
                                   yearOfData INT NOT NULL,
                                   institutionId INT NOT NULL REFERENCES institution(id),
                                   acreditationTypeId INT REFERENCES acreditationType(id), --Acreditación (31 de octubre de 2024)
                                   acreditation INT NULL, -- Años acreditación (31 de octubre de 2024)
                                   acreditationExpireAt DATE NULL, -- Vigencia acreditación (31 de octubre de 2024) -- "Desde 9 de marzo de 2022 hasta 9 de marzo de 2025" 
                                   builded DECIMAL(18,2), -- m² construidos
                                   buildedLibrary  DECIMAL(18,2), -- m² construidos biblioteca
                                   buildedLabs DECIMAL(18,2) NOT NULL, -- m² construidos laboratorios y talleres
                                   labs INT NOT NULL, -- N° de laboratorios y talleres
                                   computersPerStudent DECIMAL (18,14), -- Computadores por estudiante (Pregrado y Posgrado)
                                   greenArea DECIMAL(18,2) -- m² áreas verdes y esparcimiento
);

CREATE TABLE knowledgeArea (
                               id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                               name VARCHAR(255) NOT NULL,
                               isDeleted BOOLEAN NOT NULL
);

INSERT INTO knowledgeArea (name, isDeleted)
VALUES
    ( 'Administración y Comercio', FALSE),
    ( 'Agropecuaria', FALSE),
    ( 'Arte y Arquitectura', FALSE),
    ( 'Ciencias Básicas', FALSE),
    ( 'Ciencias Sociales', FALSE),
    ( 'Derecho', FALSE),
    ( 'Educación', FALSE),
    ( 'Humanidades', FALSE),
    ( 'Salud', FALSE),
    ( 'Tecnología', FALSE);


CREATE TABLE career (
                        id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                        name VARCHAR(255) NOT NULL,
                        knowledgeAreaId INT NOT NULL REFERENCES knowledgeArea(id)
);

CREATE TABLE schedule (
                          id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                          name VARCHAR(100) NOT NULL,
                          isDeleted BOOLEAN NOT NULL
);

INSERT INTO schedule ( name, isDeleted)
VALUES
    ( 'A Distancia', FALSE),
    ( 'Diurno', FALSE),
    ( 'Otro', FALSE),
    ( 'Semipresencial', FALSE),
    ( 'Vespertino', FALSE);

CREATE TABLE region (
                        id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                        name VARCHAR(255) NOT NULL,
                        isDeleted BOOLEAN NOT NULL
);

INSERT INTO region (name, isDeleted)
VALUES
    ( 'Región de Antofagasta', FALSE),
    ( 'Región de Arica y Parinacota', FALSE),
    ( 'Región de Atacama', FALSE),
    ( 'Región de Aysen del G. C. Ibañez', FALSE),
    ( 'Región de Coquimbo', FALSE),
    ( 'Región de la Araucanía', FALSE),
    ( 'Región de los Lagos', FALSE),
    ( 'Región de los Ríos', FALSE),
    ( 'Región de Magallanes', FALSE),
    ( 'Región de Ñuble', FALSE),
    ( 'Región de Tarapacá', FALSE),
    ( 'Región de Valparaíso', FALSE),
    ( 'Región del Bío-Bío', FALSE),
    ( 'Región del Lib. B. O´Higgins', FALSE),
    ( 'Región del Maule', FALSE),
    ( 'Región Metropolitana', FALSE);

-- SEDES -> Institucion-Campus

CREATE TABLE institutionCampus (
                                   id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                                   name VARCHAR(255) NOT NULL,
                                   isDeleted BOOLEAN NOT NULL,
                                   regionId INT REFERENCES region(id),
                                   institutionId INT REFERENCES institution(id)
);

-- Carreras por institucion
CREATE TABLE careerInstitution (
                                   id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                                   name VARCHAR(255) NOT NULL,
                                   isDeleted BOOLEAN NOT NULL,
                                   careerId INT NOT NULL REFERENCES career(id),
                                   institutionId INT NOT NULL REFERENCES institution(id)
);

-- Carrera por institucion/sede
CREATE TABLE careerCampus (
                              id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                              name VARCHAR(255) NOT NULL,
                              isDeleted BOOLEAN NOT NULL,
                              careerInstitutionId INT NOT NULL REFERENCES careerInstitution(id),
                              scheduleId INT NOT NULL REFERENCES schedule(id),
                              institutionCampusId INT NOT NULL REFERENCES institutionCampus(id)
);

-- ponderacion, datos de ingresados, costo y duracion de la carrera

CREATE TABLE careerCampusStats (
                                   id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                                   isDeleted BOOLEAN NOT NULL,
                                   careerCampusId INT NOT NULL REFERENCES careerCampus(id),
                                   anualTuition INT NOT NULL,
                                   graduationFee INT NOT NULL,
                                   duration INT NOT NULL,
                                   maleEnrollment INT NOT NULL,
                                   femaleEnrollment INT NOT NULL,
                                   totalEnrollment INT NOT NULL,
                                   publicSchoolRate DECIMAL(18,14) NOT NULL,
                                   subsidizedSchoolRate DECIMAL(18,14) NOT NULL,
                                   privateSchoolRate DECIMAL(18,14) NOT NULL,
                                   femaleDegrees INT NOT NULL,
                                   maleDegrees INT NOT NULL,
                                   totalDegrees INT NOT NULL,
                                   firstYearEntryFrom DECIMAL(18,12), -- Rango ingreso a 1er año con PAES 2023 -- 80% <= X <= 100%
                                   firstYearEntryTo DECIMAL(18,12), -- Rango ingreso a 1er año con PAES 2023 -- 80% <= X <= 100%
                                   avargePaes DECIMAL(18,12) NOT NULL,-- Promedio PAES 2023 de Matrícula 1er año 2023 -- 858.458904109589
                                   avarageEnrollment DECIMAL(18, 14) NOT NULL, -- Promedio NEM 2023 de Matrícula 2023 -- 6.69333333333333
                                   vacanciesFirstSemester INT NOT NULL, -- Vacantes 1er semestre -- 80
                                   nem INT NOT NULL,-- Nem -- 20
                                   ranking INT NOT NULL,-- Ranking -- 20
                                   paesLanguaje INT NOT NULL,-- PAES Lenguaje --10
                                   paesMaths INT NOT NULL,-- PAES Matematicas -- 25
                                   paesMaths2 INT NULL,-- PAES Matematicas 2 -- 10
                                   paesHistory INT NULL,-- PAES Historia -- 10
                                   paesSciences INT NULL, -- PAES Ciencias -- 10
                                   others INT NULL, -- Otros -- 0
                                   yearOfData INT NOT NULL
);

-- empleabilidad y ingresos

CREATE TABLE careerInstitutionStats (
                                        id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                                        isDeleted BOOLEAN NOT NULL,
                                        careerInstitutionId INT REFERENCES careerInstitution(id),
                                        studyContinuity DECIMAL(18,15) NULL, -- % titulados con continuidad de estudios -- 0.658879904162923%
                                        retentionFirstYear DECIMAL(18,14) NULL, -- Retención 1er año -- 81.1042524005487%
                                        realDuration DECIMAL(3,2) NULL, -- Duración Real (semestres) -- 10.0593900481541
                                        employabilityFirstYear DECIMAL(18,14) NULL, -- Empleabilidad 1er año 80.4747320061256%
                                        employabilitySecondYear DECIMAL(18,14) NULL, -- Empleabilidad 2° año 89.9152164212405%
                                        avarageSalaryfrom INT NULL, -- Ingreso Promedio al 4° año -- De $1 millón 700 mil a $1 millón 800 mil
                                        avarageSalaryTo INT NULL,
                                        yearOfData INT NOT NULL
);