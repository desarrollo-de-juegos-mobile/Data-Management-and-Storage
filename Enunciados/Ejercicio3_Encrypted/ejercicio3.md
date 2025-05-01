# Ejercicio 3: Implementación de guardado encriptado

## Objetivos

- Implementar un sistema de guardado con encriptación para proteger los datos
- Entender los principios básicos de seguridad de datos en juegos
- Utilizar algoritmos criptográficos para proteger información sensible

## Descripción

En este ejercicio, mejorarás el sistema de guardado JSON añadiendo encriptación AES para evitar la manipulación no autorizada de archivos guardados. También implementarás verificación de integridad mediante checksums para detectar modificaciones en los datos.

## Tareas

1. Completa la clase `EncryptedSaveSystem_Base.cs` implementando:
   - Métodos de encriptación y desencriptación usando el algoritmo AES
   - Generación y verificación de checksums para comprobar la integridad
   - Métodos para guardar y cargar datos encriptados
   - Manejo de errores en caso de datos corrompidos o modificados

2. Implementa una función para corromper intencionadamente los datos guardados con fines de prueba

3. Ejecuta los tests proporcionados en `EncryptedSaveSystemTests.cs` para verificar tu implementación

## Criterios de evaluación

- El sistema encripta correctamente los datos antes de guardarlos
- El sistema desencripta correctamente los datos al cargarlos
- El sistema genera y verifica checksums para detectar modificaciones
- El sistema maneja adecuadamente errores y datos corrompidos
- La UI informa del estado de las operaciones (éxito, error, corrupción)
- Los tests automáticos se ejecutan sin errores

## Entrega

- Script `EncryptedSaveSystem.cs` completado
- Escena configurada con la implementación funcional
- Captura de pantalla que muestre los tests ejecutados exitosamente

## Recursos adicionales

- [Documentación de System.Security.Cryptography](https://docs.microsoft.com/es-es/dotnet/api/system.security.cryptography)
- [Tutorial sobre encriptación AES en C#](https://docs.microsoft.com/es-es/dotnet/api/system.security.cryptography.aes)
- [Guía sobre checksums SHA-256](https://docs.microsoft.com/es-es/dotnet/api/system.security.cryptography.sha256)
