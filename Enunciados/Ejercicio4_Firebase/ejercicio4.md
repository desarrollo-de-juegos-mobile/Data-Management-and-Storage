# Ejercicio 4: Sistema de guardado en la nube con Firebase

## Objetivos

- Implementar un sistema de guardado en la nube utilizando Firebase
- Comprender los principios de sincronización y funcionamiento offline
- Gestionar autenticación de usuarios y permisos de acceso

## Descripción

Crearás un sistema que permita guardar y cargar datos del juego en Firebase Realtime Database, con soporte para funcionamiento offline y sincronización al recuperar la conexión. Este sistema permitirá a los jugadores acceder a sus datos desde cualquier dispositivo.

## Pre-requisitos

- Cuenta de Firebase (puedes crear una gratuita en [firebase.google.com](https://firebase.google.com/))
- Firebase SDK para Unity (incluido en el material del ejercicio)

## Tareas

1. Configura Firebase en Unity siguiendo las instrucciones proporcionadas:
   - Crea un proyecto de Firebase
   - Configura la autenticación (habilita la autenticación anónima)
   - Configura Realtime Database
   - Agrega el SDK de Firebase a tu proyecto de Unity

2. Completa la clase `FirebaseSaveSystem_Base.cs` implementando:
   - Inicialización de Firebase y autenticación de usuario
   - Métodos para guardar y cargar datos en Firebase Realtime Database
   - Detección de estado de conexión y manejo de modo offline
   - Sincronización de datos cuando se recupera la conexión
   - Resolución de conflictos entre datos locales y en la nube

3. Ejecuta los tests proporcionados en `FirebaseSaveSystemTests.cs` para verificar tu implementación

## Criterios de evaluación

- El sistema se conecta correctamente a Firebase y autentica al usuario
- El sistema guarda y carga datos correctamente en Firebase Realtime Database
- El sistema detecta el estado de conexión y funciona en modo offline
- El sistema sincroniza los datos cuando se recupera la conexión
- El sistema resuelve conflictos entre datos locales y en la nube
- Los tests automáticos se ejecutan sin errores

## Entrega

- Script `FirebaseSaveSystem.cs` completado
- Configuración de Firebase completada (archivos de configuración)
- Escena configurada con la implementación funcional
- Captura de pantalla que muestre los tests ejecutados exitosamente

## Recursos adicionales

- [Documentación oficial de Firebase para Unity](https://firebase.google.com/docs/unity/setup)
- [Tutorial de Firebase Realtime Database](https://firebase.google.com/docs/database/unity/start)
- [Guía de autenticación de Firebase](https://firebase.google.com/docs/auth/unity/start)
