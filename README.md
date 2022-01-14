# The regnANT
## Repository per il progetto di Virtual Reality

# Contents

- [Storia](#Storia)
- [Storyboard](#Storyboard)
- [NPC](#NPC)


# Storia
Nascita e scelta tipo formica, opzioni tra operaio/soldato, regina e maschio, la regina e il maschio non sono presenti nella demo, oscurati

* Operaia
    * Cinematic, nascita, "formiche dottore", spiegazione di chi sei, cosa fai, ecc. da parte delle formiche che si prendono cura di te, et intanto cresci
    * Quando compare l'esoscheletro finisce la cinematica (appare la scheda esoscheletro)
    * Raccogli uova da regina (slide regina) (Tutorial) e portarle alla stanza delle uova. spiegazione dei comandi, segui le altre formiche che fanno lo stesso
1. Inizia ad accudire le larve
    * Dare da mangiare alla larva corretta in base al cibo preso da magazzino
    * cinematica formica che ripete la stessa azione per fare vedere tempo che passa
2. Più grande, operaio a tutti gli effetti
    * Vai a prendere gli afidi e li metti sulle piante poi sei libero
    * Dopo un po' di tempo incontri una formica che ti dice che hanno trovato cibo, segui i feromoni (con spiegazione sistema feromoni) e vai ad aiutare, una delle formiche durante la strada viene colpita da goccia di rugiada
    * depositi il cibo nel magazzino
3. Attacco al nido
    * Ti armi e vai a combattere per difendere il nido
    * Dopo battaglia estenuante muori, inizia cinematica con zoom out con finale battaglia e raccolta cadaveri --> scritta o narratore che dice "Tu e altre siete cadute per la colonia, ma il nido continuerà"

# Storyboard
||
|:-----------------------------------------:|
| ![S1](/ReadmeImages/Storyboard1.png "s1") |
| ![S2](/ReadmeImages/Storyboard2.png "s2") |
| ![S3](/ReadmeImages/Storyboard3.png "s3") |
| ![S4](/ReadmeImages/Storyboard4.png "s4") |


# NPC
| ![NPC FSM](/ReadmeImages/NPCFSM.png "NPC FSM") |
|:----------------------------------------------:|
|                   *NPC FSM*                    |

- `SetRandomPointDestination()` (in `AntFSM.cs`): Ottiene una posizione random via RandomNavMeshLocation() in un certo walkRadius e la imposta come destinazione
- `LookAround()` (in `AntFSM.cs`): Lancia uno spherecast per identificare il tipo di oggetto (food, pheromone, enemy) di fronte a sé
- `moveToFood()` (in `AntFSM.cs`): 
- `spawnNewPheromoneTrace()` (in `AntFSM.cs`): 
- `followPheromoneTraceToNestState()` (in `AntFSM.cs`): 
- `followPheromoneTrace()` (in `AntFSM.cs`): 



# Attributions

Book cover in diary system to: <a href="https://www.vecteezy.com/free-vector/ornament">Ornament Vectors by Vecteezy</a>