# TP_SudokuIA

Bonjour !

Bienvenue dans ce repository.

Ce repository comprend deux projets rassemblant une structure de résolution de base que vous pourrez implémenter 
en fonction de votre méthode de résolution, et une note comprenant ces instructions ainsi qu'une définition du Sudoku.

Ainsi, vous trouverez ici toutes les instructions nécessaires afin de débuter votre travail pour le TP1.

Instrcuctions :

1) Créer un fork de ce repository.

2) Ouvrir le repository sur Visual Studio Code.

3) Vous pouvez alors commencer à coder. Le but est d'adapter le solveur de base de ce repository à votre méthode.



Pour le moment, les projets sont vides. Ils seront complétés très bientôt.





Avant de commencer : Qu'est-ce qu'un Sudoku ?

Le Sudoku est un jeu de logique d'origine japonaise consistant à compléter une grille avec des nombres de 1 à 9.
Cette grille est composée de neuf carrés eux même composés de neuf cases.
Au début du jeu, ces cases sont complétées de quelques chiffres dispersés selon le niveau choisi par l'utilisateur
afin de pouvoir commencer le jeu.

De plus, un chiffre ne doit jamais se répéter au sein d'un même carré, d'une même ligne et d'une même colonne de la grille.

Pour lancer SolveurNeuralNet, tout se fait par des commandes
1) créer un environnement en python 3.6 sur Anaconda avec la commande : (terminal MacOS ou prompt Anaconda) 
conda create -n nomenvironnement python=3.6

2) activer l'environnement
conda activate nomenvironnement

3) installer les bibliothèques keras et keras-gpu
conda install keras
conda install keras-gpu

4) installer Cuda et cudnn
conda install cudatoolkit
conda install cudnn

(Trouvez le fichier cudart64_100.dll et placez le dans le dossier nomenvironnement directement)

5) Changez le chemin de distribution dans le fichier "SolveurNeuralNet.cs", ligne 44 dans le "distributionPath" par le chemin qui mène à l'environnement que vous avez crée.


