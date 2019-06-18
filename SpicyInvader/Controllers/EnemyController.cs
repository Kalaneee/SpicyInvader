using SpicyInvader.Actors;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpicyInvader.Controllers
{
    public class EnemyController
    {
        private string _enemyTexture;

        private int _firstEnemyX;
        private int _firstEnemyY;
        private Enemy[,] _enemies;
        private int _initialNbRow;
        private int _initialNbCol;
        private int _nbColEnemies;
        private SoundController _soundPlayer;

        public EnemyController(string texture, int nbRowsEnemies, int nbColsEnemies, int firstEnemyX, int firstEnemyY)
        {
            _enemyTexture = texture;

            _firstEnemyX = firstEnemyX;
            _firstEnemyY = firstEnemyY;

            _initialNbRow = nbRowsEnemies;
            _initialNbCol = nbColsEnemies;

            _soundPlayer = new SoundController("enemiesPop.wav");
        }

       /// <summary>
       ///  Create enemies and add them to Actor's List
       /// </summary>
       /// <param name="actors"></param>
        public void CreateEnemies(ref List<Actor> actors)
        {
            _nbColEnemies = _initialNbCol;
            _enemies = new Enemy[_initialNbRow, _initialNbCol];

            for (int row = 0; row < _initialNbRow; row++)
            {
                for (int col = 0; col < _initialNbCol; col++)
                {
                    _enemies[row, col] = new Enemy(_enemyTexture, _firstEnemyY + (2 * row), _firstEnemyX + ((_enemyTexture.Length + 1) * col), row, col, _initialNbCol);
                    actors.Add(_enemies[row, col]);
                }
            }

            _soundPlayer.Play(false);
        }

        public void Update()
        {
            // If there is only one column of Enemies, do nothing
            if (_enemies.GetLength(1) == 1)
            {
                return;
            }

            // If the first column of Enemies is down
            if (CheckColEnemies(0))
            {
                ResizeEnemiesArray(true);
            }
            // If the last column of Enemies is down
            else if (CheckColEnemies(_nbColEnemies - 1)) {
                ResizeEnemiesArray(false);
            }
        }

        /// <summary>
        ///  Check if the passed column of array is empty
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private bool CheckColEnemies(int col)
        {
            for (int row = 0; row < _enemies.GetLength(0); row++)
            {
                if(!_enemies[row, col].IsRemoved)
                {
                    return false;
                }
            }
            return true;
        }

        private void ResizeEnemiesArray(bool firstCol)
        {
            // If we remove the first column
            if(firstCol)
            {
                Enemy[,] resizedEnemies = new Enemy[_enemies.GetLength(0), _enemies.GetLength(1) - 1];

                // Move all enemies 1 to the left
                for (int i = 0; i < _enemies.GetLength(0); i++)
                {
                    for (int j = 1; j < _enemies.GetLength(1); j++)
                    {
                        _enemies[i, j].XPosSwarm--;
                        _enemies[i, j].XSizeSwarm--;
                        resizedEnemies[i, j - 1] = _enemies[i, j];
                    }
                }
                _enemies = resizedEnemies;
            }
            // If we remove the last column
            else
            {
                foreach (Enemy e in _enemies)
                {
                    e.XSizeSwarm--;
                }
            }
            _nbColEnemies--;
        }
    }
}
