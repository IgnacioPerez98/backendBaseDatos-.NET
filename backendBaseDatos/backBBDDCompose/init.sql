/*Script que se ejecuta cuando se crea el contenedor de MySql*/
/*Usuario para conectarse*/
CREATE USER 'sa'@'%' IDENTIFIED WITH mysql_native_password BY 'proyecto';
GRANT USAGE ON *.* TO 'sa'@'%';
ALTER USER 'sa'@'%' REQUIRE NONE WITH MAX_QUERIES_PER_HOUR 0 MAX_CONNECTIONS_PER_HOUR 0 MAX_UPDATES_PER_HOUR 0 MAX_USER_CONNECTIONS 0;
GRANT ALL PRIVILEGES ON proyectobbdd.* TO 'sa'@'%';
FLUSH PRIVILEGES;

/*Tablas*/
SELECT SCHEMA_NAME
FROM INFORMATION_SCHEMA.SCHEMATA
WHERE SCHEMA_NAME = 'proyectobbdd';

CREATE SCHEMA IF NOT EXISTS proyectobbdd;

USE proyectobbdd;

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'proyectobbdd' AND TABLE_NAME = 'logins';

CREATE TABLE IF NOT EXISTS logins(
    logid int primary key not null ,
    password varchar(255) not null
);

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'proyectobbdd' AND TABLE_NAME = 'funcionarios';

CREATE TABLE IF NOT EXISTS funcionarios(
    ci varchar(255) primary key not null ,
    nombre varchar(255),
    apellido varchar(255),
    fch_nac date,
    direccion varchar(255),
    telefono varchar(45),
    email varchar(255),
    logid int unique not null,
    foreign key (logid) references logins(logid)
);

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'proyectobbdd' AND TABLE_NAME = 'agenda';

CREATE TABLE IF NOT EXISTS agenda(
    nro varchar(255) primary key not null,
    ci varchar(255),
    fch_agenda datetime,
    foreign key (ci) references funcionarios(ci)
);

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'proyectobbdd' AND TABLE_NAME = 'carnet_salud';

CREATE TABLE IF NOT EXISTS carnet_salud(
    ci varchar(255) not null primary key ,
    fch_emision date,
    fch_vencimiento date,
    comprobante blob,
    foreign key (ci) references funcionarios(ci)
);

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'proyectobbdd' AND TABLE_NAME = 'periodos_actualizacion';

CREATE TABLE IF NOT EXISTS periodos_actualizacion(
    anio int not null ,
    semestre int not null ,
    fch_inicio date not null ,
    fch_fin date not null,
    primary key (fch_inicio, fch_fin)
);

USE proyectobbdd;

INSERT INTO logins (logid, password) VALUES (0, '91aac10dc64306f3f2be458902f3787f632b8d84428c084c7e508f827fa6a56eaf74db4354ebdbcd47682c1077846f1da03c60849c8a4cba74fb05503069e692');

INSERT INTO funcionarios (ci, nombre, apellido, fch_nac, direccion, telefono, email, logid)
VALUES ('49765166', 'Ignacio', 'Perez', '1998-04-09', 'Casucha 1234', '098110564', 'nachopp98@gmail.com', 0);

INSERT INTO periodos_actualizacion(anio, semestre, fch_inicio, fch_fin) values (2023, 2, '2023-11-01','2023-11-15')