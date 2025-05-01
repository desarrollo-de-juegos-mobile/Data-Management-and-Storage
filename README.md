# Ejercicios Prácticos - Unidad 5: Manejo de Datos y Almacenamiento

Este repositorio contiene los ejercicios prácticos para la Unidad 5 del curso "Desarrollo de Juegos Mobile" en UADE. Estos ejercicios están diseñados para ayudarte a comprender e implementar diferentes métodos de almacenamiento y gestión de datos en juegos móviles.

## Estructura del Repositorio

```
/
├── Enunciados/
│   ├── Ejercicio1_PlayerPrefs.md
│   ├── Ejercicio2_JSON.md
│   ├── Ejercicio3_Encrypted.md
│   ├── Ejercicio4_Firebase.md
│   ├── Ejercicio5_Economy.md
│   └── Ejercicio6_CloudSave.md
│
├── Tests/
│   ├── Ejercicio1_Tests/
│   ├── Ejercicio2_Tests/
│   ├── Ejercicio3_Tests/
│   ├── Ejercicio4_Tests/
│   ├── Ejercicio5_Tests/
│   └── Ejercicio6_Tests/
│
├── Soluciones/
│   ├── Ejercicio1_PlayerPrefs/
│   ├── Ejercicio2_JSON/
│   ├── Ejercicio3_Encrypted/
│   ├── Ejercicio4_Firebase/
│   ├── Ejercicio5_Economy/
│   └── Ejercicio6_CloudSave/
│
└── README.md
```

## Contenido de los Ejercicios

### [Ejercicio 1: Sistema básico de guardado con PlayerPrefs](./Enunciados/Ejercicio1.md)
Implementación de un sistema simple de guardado utilizando PlayerPrefs de Unity, perfecto para configuraciones básicas y datos simples.

### [Ejercicio 2: Serialización de datos de juego a JSON](./Enunciados/Ejercicio2.md)
Sistema de guardado más complejo usando serialización JSON para almacenar datos estructurados como perfiles de jugador, inventarios y estado del juego.

### [Ejercicio 3: Implementación de guardado encriptado](./Enunciados/Ejercicio3.md)
Mejora del sistema de guardado JSON añadiendo encriptación AES y verificación de integridad con checksums para evitar manipulaciones.

### [Ejercicio 4: Sistema de guardado en la nube con Firebase](./Enunciados/Ejercicio4.md)
Implementación de un sistema que permite guardar y cargar datos del juego en Firebase Realtime Database, con soporte para funcionamiento offline y sincronización.

### [Ejercicio 5: Sistema económico para juego F2P](./Enunciados/Ejercicio5.md)
Sistema que gestiona dos tipos de monedas (soft currency y hard currency), transacciones de compra, y verificación de integridad de datos.

### [Ejercicio 6: Sistema de guardado en la nube con Unity Cloud Save](./Enunciados/Ejercicio6.md)
Implementación de un sistema para guardar datos en la nube utilizando Unity Cloud Save, permitiendo sincronización de progreso entre dispositivos.

## Instrucciones para Estudiantes

1. **Descarga el material base**:
   - Clona este repositorio y abre el proyecto en Unity
   - Cada ejercicio tiene su propia carpeta con los archivos necesarios

2. **Lee el enunciado correspondiente**:
   - Cada ejercicio tiene un archivo markdown en la carpeta `Enunciados`
   - Sigue las instrucciones y completa las tareas indicadas

3. **Completa la implementación**:
   - Trabaja sobre los scripts base proporcionados
   - Implementa la funcionalidad requerida

4. **Verifica tu implementación**:
   - Utiliza los tests proporcionados para verificar tu trabajo
   - Cada carpeta de Tests contiene scripts para comprobar automáticamente tu implementación

5. **Entrega tu solución**:
   - Sube tu proyecto completo a tu repositorio personal
   - Asegúrate de incluir todas las capturas de pantalla y evidencias requeridas

## Notas Importantes

- Los ejercicios deben completarse en orden, ya que cada uno construye sobre el conocimiento del anterior
- La carpeta `Soluciones` contiene implementaciones de referencia que puedes consultar **después** de haber intentado resolver los ejercicios por tu cuenta
- Para los ejercicios que requieren servicios externos (Firebase, Unity Cloud Save), sigue las instrucciones detalladas en los enunciados para configurar correctamente tu proyecto

## Requisitos Técnicos

- Unity 2020.3 LTS o superior
- Para el Ejercicio 4: Firebase SDK para Unity
- Para el Ejercicio 6: Unity Gaming Services activado y configurado

## Recursos Adicionales

- [Documentación oficial de Unity](https://docs.unity3d.com/)
- [Documentación de Firebase para Unity](https://firebase.google.com/docs/unity/setup)
- [Documentación de Unity Gaming Services](https://docs.unity.com/ugs/en-us/manual/cloud-save/manual)

## Contacto

Si tienes dudas o problemas con los ejercicios, puedes:
- Consultar en clase
- Abrir un issue en este repositorio
- Contactar por email: [email@profesor.com]
