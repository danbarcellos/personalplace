# Personal Place

### Detalhes de Implementação
##### Versão 0.5.1 Alpha
* #### Plataforma .NET Core 2.1
  * API REST por meio de ASP.Net Core
  * Banco de Dados SQL Server 2017
  * ORM NHibernate 5.1.3
  * ORM Dapper
  * Injeção de Dependência com Autofac
  * Mapeamento de Objetos com AutoMapper
  * Infraestrurua de Log com NLog
  * SPA Ionic/Angular/TypedScript
  * Validação de dados com FluentValidation
  * Segurança com Encriptação/Obfuscação Rijndael/SHA512

* #### Características Técnicas
  * Princípios SOLID
  * DDD (Domain Drive Desing)
  * TDD (Test Driven Desing)
    * Unitário/Integrado
  * Abordagem ORM Code Fisrt
    * As entidadas são criadas no projeto `PersonalPlace.Domain.Entities` e a infraestrutura de mapeamento automático reflete a entidade criada e seu mapeamento no data schema, sem a necessidade de notação (atributos), XML ou outros artifícios, as entidades são POCO em absoluto.
    * Detalhes de mapeando fino podem ser personalizados em `PersonalPlace.Domain.Base.ORM`
  * Compatível com Windows e Linux
  * App Hybrid Ionic/Cordova
  
### Roadmap

* #### Junho/2018 (versão 0.8.0 beta)
  * Análise e projeto arquitetural
  * Desenvolvimento do Product Backlog e artefatos Agile
  * Abertura de repositório Git do projeto 
  * Classes infraestruturais
    * REST
    * ORM
    * Log
    * Validação
    * Mapeamento de objetos
  * MVP
* #### Setembro/2018 (versão 0.9.0 beta)
  * Database Migration
  * Cache distribuído
  * Container Docker 
  * Teste de produção
  * Aplicação Mobile Ionic
  * Monitoramento por Logstash
* #### Novembro/2018 (versão 1.0.0 beta)
  * Features finais
  * Teste de carga
  * Release candidato
  * Entrega
