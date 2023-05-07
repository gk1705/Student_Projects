Was ben�tigt man zum Spielen:
2 Xbox Controller

Software Aufbau/Architektur
Das Game hat einen GameStateManager. Man startet im MenuState, kann von dort aus das Spiel wieder schlie�en, oder in den HeroSelectionState wechseln. Dort w�hlt dann jeder Spieler einen
Hero aus. Sobald beide Heros ausgew�hlt wurden, wird die init() vom MainState aufgerufen. 

In der Init() werden dann die Spielfelder erstellt, das Level geladen mit der TileMap Klasse.
Die Helden werden geladen mit dem Heroloader, je nachdem was ausgesucht wurde. Die Monster werden mit dem EnemyLoader erstellt und auf die Position -1000,-1000 gesetzt.(wenn sie w�hrend
der Laufzeit erstellt werden, gab es kleine Ruckler im Spiel). In dem Vector<SpawnData> werden dann alle Informationen gespeichert, die f�r den Spawn des Monsters ben�tigt werden. Daf�r
haben wir eine Struct angelegt. Sobald dann eine darin festgelegte Zeit vor�ber ist, werden die Monster von der Position -1000, -1000 ins Spielfeld gesetzt.

Unsere GameObject haben je nachdem, welche Funktion sie erledigen sollen verschiedene Components oder Observer. 
Zum Beispiel wenn wir eine Magiekugel erstellen, wird darauf ein MagicBulletComponent gelegt und ein MagicBulletObserver. Der Component bewegt die Kugel in eine Richtung und der Observer 
ist daf�r zust�ndig, Kollisionen mit verschiedenen Objekten aufzul�sen. Also wenn die Kugel mit einem gegnerischen Monster zusammenst��t, wird das gegnerische Monster schneller. 
Wenn die Kugel mit einem Monster im eigenen Feld zusammenst��t, wird es langsamer.

Generell haben wir nat�rlich viel mit Components gearbeitet, aber auch sehr viel mit Manager(die Singletons sind), wie z.B. den GameObjectManager. Auch Observer wurden sehr oft verwendet,
um z.B. Kollisionen verschieden zu behandeln oder f�r Lebensbalken oder Ultimatebalken.

Man kann dann noch im Spiel in den PauseState wechseln. Dort kann man zur�ck ins Men� oder zur�ck ins Game. Und wenn das Spiel vorbei ist, also ein Spieler 0 Leben hat, wird in den
GameOverState gewechselt.

Die Core-Mechanic wurde mithilfe eines PlayfieldManagers implementiert.
Dieser speichert, welche Objekte sich in welchem Feld befinden. Das erm�glicht uns, zu erfragen, welches Objekt sich momentan wo befindet.
Somit ist es einfach, zu erfragen, wo Objekte im selben Spielfeld sind. (Monster reagieren nur auf den Spieler im selben Spielfeld). 
Weiters �bernimmt der Manager das Wechseln von Objekten zwischen Spielfeldern.