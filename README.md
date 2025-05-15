# 🛡️ Solución de Gestión de Permisos

Esta aplicación ha sido desarrollada con **.NET 8** y tiene como objetivo proporcionar una **API RESTful** para la gestión de permisos en una base de datos. La solución está diseñada siguiendo principios de arquitectura limpia y buenas prácticas de desarrollo.

---

## 🚀 Tecnologías Utilizadas

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Entity Framework Core 8
- Apache Kafka
- Elasticsearch

---

## 🏗️ Arquitectura

Se implementó **arquitectura limpia** basada en cinco capas bien definidas:

- **Domain**  
  Contiene las entidades e interfaces.

- **Infrastructure**  
  Proporciona la implementación de servicios como acceso a base de datos, Kafka, Elasticsearch.

- **Application**  
  Contiene las reglas de negocio e implementacion CQRS.

- **WebApi**  
  Capa de presentación que expone los endpoints REST.
  
- **Test**  
  Capa para pruebas unitarias y de integracion.
  
---

## 🧩 Patrones de Diseño Utilizados

- **Repository**  
  Para abstraer el acceso a los datos y facilitar pruebas unitarias.

- **Unit of Work**  
  Para manejar transacciones y asegurar la consistencia de los datos.

- **CQRS (Command Query Responsibility Segregation)**  
  Separación clara entre operaciones de lectura y escritura.

---

¡Con esta solución se busca robustez, escalabilidad y mantenibilidad para una gestión eficiente de permisos! 💼🔐
