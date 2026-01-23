#### Tarea: Implementar un player controler 3ra persona
* El movimiento debe ser fluido y responsivo
* Cámara estable y cómoda
* Colisiones confiables

#### Interacciones: Talar un árbol y transportar recursos
* Movimiento 
* Control de las manos/brazos
* Tomar herramienta
* Soltar herramienta
* Blandir el hacha
* Empujar objetos sueltos

#### Ideas de control del jugador
* WASD + IJKL: WASD para mover adelante/atrás y strafe, IJKL para levantar/bajar los brazos y rotar el torso. Ayudado de shift u otra tecla para rotar con mayor velocidad, para tareas como talar o picar.
* WASD + Mouse: WASD para movimiento adelante/atrás, izquierda/derecha, con el mouse se controlan los brazos. Para esto, hay botones para mantener quieto el cuerpo del jugador, permitiendo fijar un eje y revolear el mouse para darle velocidad en el otro.
* WASD + Mouse 2: Idea principal como antes, pero se puede hacer "zoom" a una cámara sobre el hombro del jugador al entrar en interacción con un objeto con tarea. P.ej. acercarse al árbol y tocar E entra en una nueva cámara y sólo se toma input del mouse hasta terminar de talar el árbol o elegir dejar la actividad.

#### Cámaras:
* Top-down fija, de todo el escenario con posible zoom, estilo Overcooked/PlateUp
* A la altura, siguiendo la acción desde ángulo fijo al escenario. Como en Gang Beasts
* Over the shoulder: No creo, se manejaría con la orientación del jugador y el mouse, y si se usa eso para sacudir el hacha hay que compensar los giros, meter dampen

Assets de verdad que necesito:
* Un árbol con color de árbol
* Troncos con un mesh simple
* Un hacha
* Algún monigote con brazos animables por código
* Texturas de pasto y madera
* SFX: Pedazos de madera que saltan o chispas que pueda pintar de marrón. Alguna nubecita o partícula para cuando cambia el árbol por troncos.


Uso cámaras del pack Cinemachine para poder arrancar más rápido a programar el controlador.

