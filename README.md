# MediatorItEasy

Una implementación ligera del patrón Mediator en .NET, inspirada en la publicación de [Cihan Elibol](https://medium.com/@cihanelibol99/building-your-own-lightweight-mediator-in-net-goodbye-mediatr-3ef19feb7576), con mejoras de rendimiento al evitar el uso de reflexión mediante `ConcurrentDictionary`.

---

## 🚀 Objetivo

Explorar la creación de un Mediador casero sin depender de bibliotecas externas, destacando:

- Separación de responsabilidades.
- Resolución de handlers vía DI con o sin reflexión.
- Uso de `ConcurrentDictionary` para invocar sin penalizaciones de `MethodInfo.Invoke`.

---

## 🧠 Motivación

Jimmy Bogard anunció recientemente que MediatR será de pago en versiones futuras. Estoy completamente de acuerdo con la decisión —el software libre no se mantiene solo— pero decidí aprovechar el momento para aprender más sobre el patrón y los límites de la optimización.
