Was benötigt man zum Spielen:
2 Xbox Controller

Software Aufbau/Architektur
Das Game hat einen GameStateManager. Man startet im MenuState, kann von dort aus das Spiel wieder schließen, oder in den HeroSelectionState wechseln. Dort wählt dann jeder Spieler einen
Hero aus. Sobald beide Heros ausgewählt wurden, wird die init() vom MainState aufgerufen. 

In der Init() werden dann die Spielfelder erstellt, das Level geladen mit der TileMap Klasse.
Die Helden werden geladen mit dem Heroloader, je nachdem was ausgesucht wurde. Die Monster werden mit dem EnemyLoader erstellt und auf die Position -1000,-1000 gesetzt.(wenn sie während
der Laufzeit erstellt werden, gab es kleine Ruckler im Spiel). In dem Vector<SpawnData> werden dann alle Informationen gespeichert, die für den Spawn des Monsters benötigt werden. Dafür
haben wir eine Struct angelegt. Sobald dann eine darin festgelegte Zeit vorüber ist, werden die Monster von der Position -1000, -1000 ins Spielfeld gesetzt.

Unsere GameObject haben je nachdem, welche Funktion sie erledigen sollen verschiedene Components oder Observer. 
Zum Beispiel wenn wir eine Magiekugel erstellen, wird darauf ein MagicBulletComponent gelegt und ein MagicBulletObserver. Der Component bewegt die Kugel in eine Richtung und der Observer 
ist dafür zuständig, Kollisionen mit verschiedenen Objekten aufzulösen. Also wenn die Kugel mit einem gegnerischen Monster zusammenstößt, wird das gegnerische Monster schneller. 
Wenn die Kugel mit einem Monster im eigenen Feld zusammenstößt, wird es langsamer.

Generell haben wir natürlich viel mit Components gearbeitet, aber auch sehr viel mit Manager(die Singletons sind), wie z.B. den GameObjectManager. Auch Observer wurden sehr oft verwendet,
um z.B. Kollisionen verschieden zu behandeln oder für Lebensbalken oder Ultimatebalken.

Man kann dann noch im Spiel in den PauseState wechseln. Dort kann man zurück ins Menü oder zurück ins Game. Und wenn das Spiel vorbei ist, also ein Spieler 0 Leben hat, wird in den
GameOverState gewechselt.

Die Core-Mechanic wurde mithilfe eines PlayfieldManagers implementiert.
Dieser speichert, welche Objekte sich in welchem Feld befinden. Das ermöglicht uns, zu erfragen, welches Objekt sich momentan wo befindet.
Somit ist es einfach, zu erfragen, wo Objekte im selben Spielfeld sind. (Monster reagieren nur auf den Spieler im selben Spielfeld). 
Weiters übernimmt der Manager das Wechseln von Objekten zwischen Spielfeldern.