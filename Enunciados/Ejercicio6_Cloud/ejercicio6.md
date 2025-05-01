# Ejercicio 6: Sistema de guardado en la nube con Unity Cloud Save

## Objetivos

- Implementar un sistema de guardado utilizando Unity Cloud Save
- Integrar servicios de Unity Gaming Services en un proyecto
- Comprender la sincronización de datos entre dispositivos

## Descripción

Crearás un sistema que permita guardar y cargar datos del juego utilizando Unity Cloud Save, permitiendo a los jugadores sincronizar su progreso entre diferentes dispositivos. Implementarás la autenticación de usuario y gestionarás la sincronización de datos.

## Pre-requisitos

- Cuenta de Unity (la misma que usas para el Editor)
- Proyecto registrado en Unity Gaming Services

## Tareas

1. Configura Unity Gaming Services en tu proyecto:
   - Abre la ventana de Project Settings > Services
   - Vincula tu proyecto a Unity Gaming Services
   - Habilita Cloud Save y Authentication
   - Instala los paquetes necesarios desde el Package Manager

2. Completa la clase `CloudSaveSystem_Base.cs` implementando:
   - Inicialización de Unity Services y autenticación de usuario
   - Métodos para guardar y cargar datos utilizando Cloud Save
   - Actualización de la UI para mostrar el estado de sincronización
   - Manejo de errores y situaciones excepcionales

3. Ejecuta los tests proporcionados en `CloudSaveSystemTests.cs` para verificar tu implementación

## Criterios de evaluación

- El sistema inicializa correctamente Unity Services y autentica al usuario
- El sistema guarda y carga datos correctamente en Unity Cloud Save
- La UI muestra claramente el estado de sincronización y los datos del jugador
- El sistema maneja adecuadamente errores y situaciones excepcionales
- Los tests automáticos se ejecutan sin errores

## Entrega

- Script `CloudSaveSystem.cs` completado
- Configuración de Unity Gaming Services completada
- Escena configurada con la implementación funcional
- Captura de pantalla que muestre los tests ejecutados exitosamente

## Recursos adicionales

- [Documentación oficial de Unity Cloud Save](https://docs.unity.com/ugs/manual/cloud-save/manual)
- [Tutorial de Unity SDK para Cloud Save](https://docs.unity.com/ugs/manual/cloud-save/manual/tutorials/unity-sdk)
- [Ejemplos de Unity Cloud Save](https://docs.unity.com/ugs/manual/cloud-save/manual/tutorials/unity-sdk-sample)
