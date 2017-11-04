﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.System;

namespace platformerGame
{
    class cEntityPool : IPool
    {
        cGameScene pScene;
        List<cBullet> bullets;

        List<cMonster> monsters;

        Vector2f worldSize;

        cPlayer pPlayer;
        public cEntityPool(cGameScene scene, Vector2f world_size, cPlayer p_player)
        {
            this.pScene = scene;
            this.worldSize = world_size;
            this.pPlayer = p_player;
            this.bullets = new List<cBullet>();
            this.monsters = new List<cMonster>();
        }
        public void AddBullet(cGameObject owner, Vector2f pos, Vector2f direction)
        {
            cBullet bullet = new cBullet(owner, pos, direction);
            bullets.Add(bullet);
        }

        public void AddBullet(cBullet bullet)
        {
            bullets.Add(bullet);
        }

        public void AddMonster(cMonster monster)
        {
            this.monsters.Add(monster);
        }
        public void Update(float step_time)
        {
            

            //update bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].isActive())
                    bullets[i].Update(step_time);
                else
                    bullets.RemoveAt(i);
            }

            //update monsters
            for (int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i].isAlive())
                    monsters[i].Update(step_time);
                else
                {
                    if(monsters[i].CanThink)
                        monsters[i].Kill();
                    //monsters.RemoveAt(i);
                }
            }

            this.checkBulletVsEntityCollisions();
        }

        public void checkBulletVsEntityCollisions()
        {
            for (int b = 0; b < bullets.Count; b++)
            {
                int m = 0;
                bool collision = false;
                Vector2f intersection = new Vector2f(0.0f, 0.0f);
                while (m < monsters.Count && !collision)
                {
                   
                    collision = cCollision.testBulletVsEntity(bullets[b].Position, bullets[b].LastPosition, monsters[m].Bounds, ref intersection);
                    m++;
                }

                if (collision)
                {
                    cCollision.resolveMonsterVsBullet(monsters[m-1], bullets[b], intersection);
                }
            }
        }
        
        public int getNumOfActiveBullets()
        {
            return bullets.Count;
        }

        public void RenderBullets(RenderTarget destination, float alpha)
        {
            //draw bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].CalculateViewPos(alpha);
                bullets[i].Render(destination);
            }
        }

        public void RenderEntities(RenderTarget destination, float alpha)
        {
            

            //draw monsters
            for (int i = 0; i < monsters.Count; i++)
            {
                monsters[i].CalculateViewPos(alpha);
                monsters[i].Render(destination);
            }
        }

        public List<IDrawable> ListVisibles(cAABB view_region)
        {
            List<IDrawable> visibleObjects = new List<IDrawable>();

            for (int i = 0; i < bullets.Count; i++)
            {
                if (cCollision.OverlapAABB(bullets[i].Bounds, view_region))
                    visibleObjects.Add(bullets[i]);
            }

            return visibleObjects;

        }
    }
}
