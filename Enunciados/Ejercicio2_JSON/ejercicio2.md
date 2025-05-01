# Ejercicio 2: Serialización de datos de juego a JSON

## Objetivos

- Implementar un sistema de guardado más avanzado usando serialización JSON
- Entender la serialización y deserialización de datos estructurados
- Gestionar archivos en el sistema de almacenamiento persistente de Unity

## Descripción

Crearás un sistema de guardado para un juego RPG que utiliza serialización JSON para almacenar datos estructurados más complejos, incluyendo perfil del jugador, inventario y estado del juego. Los datos serán guardados en archivos dentro del almacenamiento persistente de Unity.

## Tareas

1. Completa las clases de datos proporcionadas:
   - `PlayerProfile` para almacenar datos del jugador (nombre, nivel, experiencia)
   - `GameItem` para representar objetos del inventario (id, nombre, cantidad)
   - `GameState` para el estado del juego (nivel actual, misiones completadas)
   - `SaveData` como contenedor principal para todos los datos anteriores

2. Implementa los métodos en `JSONSaveSystem_Base.cs`:
   - Método para serializar datos a JSON y guardarlos en un archivo
   - Método para cargar y deserializar datos desde un archivo JSON
   - Métodos de simulación para modificar datos (añadir experiencia, añadir items, etc.)
   - Actualización de la UI para mostrar los datos actuales

3. Ejecuta los tests proporcionados en `JSONSaveSystemTests.cs` para verificar tu implementación

## Criterios de evaluación

- El sistema serializa correctamente datos complejos a formato JSON
- El sistema guarda correctamente los archivos JSON en la ubicación adecuada
- El sistema carga correctamente los datos desde archivos JSON
- La UI se actualiza para reflejar el estado actual de los datos
- Los tests automáticos se ejecutan sin errores

## Entrega

- Script `JSONSaveSystem.cs` completado
- Escena configurada con la implementación funcional
- Captura de pantalla que muestre los tests ejecutados exitosamente

## Recursos adicionales

- [Documentación de JsonUtility en Unity](https://docs.unity3d.com/ScriptReference/JsonUtility.html)
- [Documentación de System.IO en C#](https://docs.microsoft.com/es-es/dotnet/api/system.io)
- [Guía de persistencia de datos en Unity](https://learn.unity.com/tutorial/persistence-saving-and-loading-data)
