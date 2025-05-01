# Ejercicio 5: Sistema económico para juego F2P

## Objetivos

- Implementar un sistema económico completo para un juego Free-to-Play
- Entender los principios de seguridad para transacciones virtuales
- Diseñar estructuras de datos para economías virtuales

## Descripción

Desarrollarás un sistema económico para un juego F2P que gestione dos tipos de monedas (soft currency y hard currency), transacciones de compra, y verificación de integridad de datos para prevenir trampas.

## Tareas

1. Completa la clase `GameEconomySystem_Base.cs` implementando:
   - Gestión de monedas virtuales (oro como soft currency y gemas como hard currency)
   - Métodos para añadir y gastar ambos tipos de monedas
   - Validación de transacciones con verificación de fondos suficientes
   - Simulación de verificación con servidor para transacciones premium
   - Guardado encriptado de datos económicos para prevenir trampas
   - Registro de transacciones para auditoría

2. Implementa la interfaz de usuario para mostrar:
   - Balance actual de monedas
   - Historial de transacciones
   - Estado de las operaciones (éxito, error, validación)

3. Ejecuta los tests proporcionados en `GameEconomySystemTests.cs` para verificar tu implementación

## Criterios de evaluación

- El sistema gestiona correctamente los dos tipos de monedas
- El sistema valida transacciones y verifica fondos suficientes
- El sistema simula verificación con servidor para compras premium
- El sistema guarda datos encriptados y verifica su integridad
- La UI muestra clara y correctamente el estado económico del jugador
- Los tests automáticos se ejecutan sin errores

## Entrega

- Script `GameEconomySystem.cs` completado
- Escena configurada con la implementación funcional
- Captura de pantalla que muestre los tests ejecutados exitosamente

## Recursos adicionales

- [Documentación de Unity sobre transacciones in-app](https://docs.unity3d.com/Manual/UnityIAP.html)
- [Mejores prácticas para economías de juegos F2P](https://www.gamedeveloper.com/business/monetization-in-mobile-games-how-to-build-a-sustainable-economy)
- [Seguridad en transacciones virtuales](https://learn.unity.com/tutorial/securing-your-game-from-cheaters)
