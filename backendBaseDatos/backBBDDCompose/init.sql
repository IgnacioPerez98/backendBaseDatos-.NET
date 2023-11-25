/*Script que se ejecuta cuando se crea el contenedor de MySql*/
/*Usuario para conectarse*/
CREATE USER 'sa'@'%' IDENTIFIED BY 'proyecto';
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
    esadmin boolean default false,
    logid int unique not null,
    foreign key (logid) references logins(logid)
);

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'proyectobbdd' AND TABLE_NAME = 'agenda';

CREATE TABLE IF NOT EXISTS agenda(
    nro int not null,
    ci varchar(255),
    fch_agenda datetime,
    estareservado bool,
    primary key (nro,fch_agenda),
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


/*nachopp98@gmail.com      clave1*/
/*admin@correo.ucu      admin*/


INSERT INTO logins (logid, password) VALUES (-1, 'a5345984926bec1df5ecf7a3ac13001713e24095e0d986eb38131ef58b5500fa15b88d7dce874a0ad838ce17c8b2d8ed99264286b29ada89a4737be09734e66b'); 
INSERT INTO logins (logid, password) VALUES (0, '91aac10dc64306f3f2be458902f3787f632b8d84428c084c7e508f827fa6a56eaf74db4354ebdbcd47682c1077846f1da03c60849c8a4cba74fb05503069e692');

INSERT INTO funcionarios (ci, nombre, apellido, fch_nac, direccion, telefono, email,esadmin, logid) VALUES ('00000000', 'Administrador', 'Administrador', '2000-01-01', '8 de Octubre', '25000000', 'admin@correo.ucu', 1,-1);
INSERT INTO funcionarios (ci, nombre, apellido, fch_nac, direccion, telefono, email,esadmin, logid) VALUES ('49765166', 'Ignacio', 'Perez', '1998-04-09', 'Casucha 1234', '098110564', 'nachopp98@gmail.com', 0, 0);

INSERT INTO periodos_actualizacion(anio, semestre, fch_inicio, fch_fin) values (2023, 2, '2023-11-01','2023-11-15')