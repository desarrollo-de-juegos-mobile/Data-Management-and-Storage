# Ejercicio 1: Sistema básico de guardado con PlayerPrefs

## Objetivos

- Implementar un sistema simple de guardado usando PlayerPrefs de Unity
- Entender el ciclo de vida de datos persistentes básicos
- Familiarizarse con operaciones básicas de entrada/salida

## Descripción

En este ejercicio crearás un sistema básico para guardar y cargar datos del juego utilizando el sistema PlayerPrefs de Unity. Implementarás guardado para un perfil de jugador simple que incluye nombre, puntuación y nivel, así como operaciones para restablecer los datos.

## Tareas

1. Completa la clase `SaveSystem_Base.cs` proporcionada implementando:
   - Métodos para guardar datos utilizando PlayerPrefs
   - Métodos para cargar datos desde PlayerPrefs
   - Un método para restablecer los datos a los valores predeterminados
   - Actualización de la UI para mostrar los valores actuales

2. Prueba tu implementación en diferentes escenarios:
   - Guardado y carga de datos
   - Cierre y reinicio de la aplicación manteniendo los datos
   - Restablecimiento de datos

3. Ejecuta los tests proporcionados en `SaveSystemTests.cs` para verificar tu implementación

## Criterios de evaluación

- El sistema guarda correctamente los datos del jugador mediante PlayerPrefs
- El sistema carga correctamente los datos guardados
- El sistema actualiza la UI para reflejar el estado actual
- El sistema permite reiniciar los datos a valores predeterminados
- Los tests automáticos se ejecutan sin errores

## Entrega

- Script `SaveSystem.cs` completado
- Escena configurada con la implementación funcional
- Captura de pantalla que muestre los tests ejecutados exitosamente

## Recursos adicionales

- [Documentación de PlayerPrefs en Unity](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html)
- [Tutorial de manejo básico de datos en Unity](https://learn.unity.com/tutorial/persistence-saving-and-loading-data)
