# The regnANT
## Repository per il progetto di Virtual Reality
# Contents

- [Storia](#Storia)
- [Storyboard](#Storyboard)
- [NPC](#NPC)
- [Boss Fight](#Boss_Fight)
- [Attributions](#Attributions)


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
|                                           |
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

# Boss_Fight

| ![spider FSM](/ReadmeImages/spiderFSM.png "spider FSM") |
|:-------------------------------------------------------:|
|                      *spider FSM*                       |

- `SetRandomPointDestination()` (in `spiderFSM.cs`): Ottiene una posizione random via RandomNavMeshLocation() in un certo walkRadius e la imposta come destinazione
- `RandomNavMeshLocation()` (in `spiderFSM.cs`): Lancia uno spherecast per identificare il tipo di oggetto (food, pheromone, enemy) di fronte a sé

# Attributions

Rigged ant model by Ryan_Plyler<a href="https://www.blendswap.com/blend/11127"> on blendswap.com</a>

Cottage model by gerhald3d<a href="https://free3d.com/3d-model/gameready-cottage-free-163528.html"> on free3d.com</a>

Tree model by atrodler<a href="https://sketchfab.com/3d-models/maple-tree-4b27adcf92f644bdabf8ecc6c5bef399"> on sketchfab.com</a>

Garden set models by Prudence<a href="https://sketchfab.com/3d-models/garden-set-d7b65635448f403ca22ffd1b27856bff"> on sketchfab.com</a>

White flower model by tojamerlin<a href="https://sketchfab.com/3d-models/white-flower-9e025b18a39741a4a38b197cee3cdcac"> on sketchfab.com</a>

3D model Rose by Stylesharp<a href="https://www.turbosquid.com/3d-models/3d-model-rose-1242547#"> on turbosquid.com</a>

Rose Branch by Lassi Kaukonen<a href="https://sketchfab.com/3d-models/rose-branch-42b44a59993847d58ae0cf5ef8cef4b7"> on sketchfab.com</a>

Flower model by 16874uy<a href="https://sketchfab.com/3d-models/flower-99237f1d475244c9aed11c9979251ced"> on sketchfab.com</a>

Tree branch model by oscarherry3d<a href="https://sketchfab.com/3d-models/tree-branch-ad3ba37cc8e946e2a429d1b5c315ce19"> on sketchfab.com</a>

Book cover in diary system to: <a href="https://www.vecteezy.com/free-vector/ornament">Ornament Vectors by Vecteezy</a>

Spider model by: misfit410 <a href="https://skfb.ly/6BGpt"> on sketchfab</a>

Mosquito model by: Branislav<a href="https://sketchfab.com/3d-models/mosquito-fce3bbae949c4966a6cfd9d757deba9b"> on sketchfab</a>

Dialogue UI by <a href="https://opengameart.org/content/ui-pack-buttons-and-dialogue">OpenGameArt</a>

Compass texture by <a href="https://www.vecteezy.com/free-vector/compass">Compass Vectors by Vecteezy</a>

Warning Icon by  <a href="https://www.flaticon.com/free-icons/warning" title="warning icons">Warning icons created by Smashicons - Flaticon</a>

Epic Boss Battle by Juhani Junkala | <a href=https://soundcloud.com/juhanijunkala>
Music promoted by <a href=https://www.free-stock-music.com>
Creative Commons Attribution 3.0 Unported License <a href=https://creativecommons.org/licenses/by/3.0/deed.en_US>

<a target="_blank" href="https://icons8.com/icon/54526/1-key">1 Key</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>