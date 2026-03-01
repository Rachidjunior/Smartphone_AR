# AR Target Practice

Application de réalité augmentée mobile développée avec Unity et AR Foundation.

## Description

AR Target Practice est un mini-jeu de tir en réalité augmentée pour Android. Le joueur utilise un marqueur physique (image imprimée ou affichée) comme contrôleur pour détruire des cibles virtuelles placées dans son environnement réel.

## Fonctionnalités

- **Détection d'images** : Reconnaissance d'un marqueur physique et ancrage d'objets 3D
- **Détection de plans** : Identification des surfaces horizontales (tables, sol)
- **Placement interactif** : Positionnement de la zone de jeu par tap sur une surface
- **Interactions spatiales** : Objet suiveur synchronisé avec le marqueur
- **Gameplay** : Système de cibles avec détection de collision et respawn automatique

## Technologies

- Unity 6000.3.6f1 LTS
- AR Foundation 5.x
- ARCore XR Plugin
- New Input System

## Prérequis

### Développement
- Unity 6000.3.6f1 ou supérieur
- Android SDK (API Level 29 minimum)
- Git

### Exécution
- Appareil Android compatible ARCore ([liste des appareils](https://developers.google.com/ar/devices))
- Android 10.0 (API 29) minimum
- Caméra fonctionnelle
- Marqueur image (fourni dans `Assets/Reference Image Library/`)

## Installation
```bash
# Cloner le dépôt
git clone https://github.com/Rachidjunior/Smartphone_AR.git

# Ouvrir le projet dans Unity Hub
# Sélectionner Unity 6000.3.6f1 LTS
```

## Configuration

1. Ouvrir le projet dans Unity
2. Importer les packages requis via Package Manager :
   - AR Foundation
   - ARCore XR Plugin
   - Android Logcat (optionnel, pour le débogage)
3. Configurer les paramètres Android dans Build Settings
4. Activer le débogage USB sur votre appareil Android

## Utilisation

### Build et déploiement

1. Connecter l'appareil Android via USB
2. File → Build Settings → Build And Run
3. Autoriser l'accès caméra lors du premier lancement

### Gameplay

1. Scanner l'environnement en bougeant lentement le téléphone
2. Taper sur une surface plane détectée pour placer la zone de jeu (GameBoard)
3. Approcher le marqueur physique du GameBoard
4. Trois cibles rouges apparaissent automatiquement
5. Déplacer le marqueur pour contrôler la balle jaune
6. Toucher les cibles pour les détruire
7. De nouvelles cibles apparaissent après destruction complète

## Structure du projet
```
Assets/
├── Scenes/
│   └── SampleScene.unity
├── Scripts/
│   ├── BoardPlacement.cs          # Placement du GameBoard
│   ├── MarkerInteraction.cs       # Interactions marker-zone
│   └── TargetManager.cs           # Logique du jeu
├── Prefabs/
│   ├── ARPlanePrefab.prefab
│   ├── GameBoard.prefab
│   ├── MarkerContent.prefab
│   ├── Target.prefab
│   └── Bullet.prefab
├── Materials/
└── Reference Image Library/
    ├── img.jpg
    └── ReferenceImageLibrary.asset
```

## Scripts principaux

- **BoardPlacement.cs** : Gère le placement de la zone de jeu via tap-to-place
- **MarkerInteraction.cs** : Orchestre les interactions entre marqueur et GameBoard
- **TargetManager.cs** : Génération procédurale des cibles et détection de collision

## Configuration matérielle de test

- **Développement** : HP EliteBook 840 G5, Linux (Ubuntu)
- **Test** : Samsung Galaxy A56, Android 14

## Améliorations futures

- Interface utilisateur AR avec score et timer
- Niveaux de difficulté progressifs
- Effets visuels et sonores
- Système de highscore persistant
- Support multijoueur local

## Rapport technique

Un rapport détaillé du projet est également disponible.

## Auteur

Emmanuel KOUASSI - Parcours RVSI

## Licence

Projet académique - Travaux Pratiques Smartphone Augmented Reality
