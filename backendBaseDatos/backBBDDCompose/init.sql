/*Script que se ejecuta cuando se crea el contenedor de MySql*/
/*Usuario*/
CREATE USER 'sa'@'%' IDENTIFIED WITH mysql_native_password BY 'proyecto';
GRANT USAGE ON *.* TO 'sa'@'%';
ALTER USER 'sa'@'%' REQUIRE NONE WITH MAX_QUERIES_PER_HOUR 0 MAX_CONNECTIONS_PER_HOUR 0 MAX_UPDATES_PER_HOUR 0 MAX_USER_CONNECTIONS 0;
GRANT ALL PRIVILEGES ON proyectoback.* TO 'sa'@'%';
FLUSH PRIVILEGES;

/*Tablas*/
SELECT SCHEMA_NAME
FROM INFORMATION_SCHEMA.SCHEMATA
WHERE SCHEMA_NAME = 'proyectoback';

CREATE SCHEMA IF NOT EXISTS proyectoback;

USE proyectoback;

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'proyectoback' AND TABLE_NAME = 'logins';

CREATE TABLE IF NOT EXISTS logins(
    logid int primary key not null ,
    password varchar(255) not null
);

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'proyectoback' AND TABLE_NAME = 'funcionarios';

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
WHERE TABLE_SCHEMA = 'proyectoback' AND TABLE_NAME = 'agenda';

CREATE TABLE IF NOT EXISTS agenda(
    nro int primary key not null auto_increment,
    ci varchar(255),
    fch_agenda datetime,
    foreign key (ci) references funcionarios(ci)
);

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'proyectoback' AND TABLE_NAME = 'carnet_salud';

CREATE TABLE IF NOT EXISTS carnet_salud(
    ci varchar(255) not null primary key ,
    fch_emision date,
    fch_vencimiento date,
    comprobante blob,
    foreign key (ci) references funcionarios(ci)
);

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'proyectoback' AND TABLE_NAME = 'periodos_actualizacion';

CREATE TABLE IF NOT EXISTS periodos_actualizacion(
    anio int not null ,
    semestre int not null ,
    fch_inicio date not null ,
    fch_fin date not null,
    primary key (fch_inicio, fch_fin)
);