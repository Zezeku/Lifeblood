using System;
using System.Collections.Generic;
using System.Diagnostics;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lifeblood;

public class Zone_Start: IScene
{
    Boolean isPlaying;

    Texture2D texture_player_idle;
    Texture2D texture_player_moving;
    Texture2D texture_player;
    Vector2 position_player;

    int pixels_per_step;
    float sec_per_step;
    int pixels_per_sec;

    enum PLAYER_STATE
    {
        IDLE,
        MOVING_UP,
        MOVING_DOWN,
        MOVING_LEFT,
        MOVING_RIGHT
    }
    PLAYER_STATE ps;

    bool IScene.IsPlaying()
    {
        return isPlaying;
    }

    void IScene.LoadContent(ContentManager content)
    {
        isPlaying = true;
        texture_player_idle = content.Load<Texture2D>("AvatarSprite");
        texture_player_moving = content.Load<Texture2D>("AvatarSpriteMove");
        texture_player = texture_player_idle;

        position_player = new Vector2(8*3*10,8*3*10); // must be divisible by 8
        pixels_per_step = (int)(texture_player.Width*(float)0.5*3); //need scale factor here
        sec_per_step = (float)0.0625;
        pixels_per_sec = (int)(pixels_per_step / sec_per_step);

        ps = PLAYER_STATE.IDLE;
    }

    void IScene.Update(GameTime gameTime)
    {       

        switch(ps)
        {
            case PLAYER_STATE.IDLE:
                texture_player = texture_player_idle;
                if(Keyboard.GetState().IsKeyDown(Keys.Up))
                    ps = PLAYER_STATE.MOVING_UP;
                
                if(Keyboard.GetState().IsKeyDown(Keys.Down))
                    ps = PLAYER_STATE.MOVING_DOWN;
                
                if(Keyboard.GetState().IsKeyDown(Keys.Left))
                    ps = PLAYER_STATE.MOVING_LEFT;
                
                if(Keyboard.GetState().IsKeyDown(Keys.Right))
                    ps = PLAYER_STATE.MOVING_RIGHT;
                break;

            case PLAYER_STATE.MOVING_UP:
                texture_player = texture_player_moving;
                if(Keyboard.GetState().IsKeyUp(Keys.Up))
                {
                    if((int)(position_player.Y - pixels_per_sec*(float)gameTime.ElapsedGameTime.TotalSeconds) > (int)(position_player.Y / pixels_per_step)*pixels_per_step)
                    {
                        position_player.Y = (int)(position_player.Y - pixels_per_sec*(float)gameTime.ElapsedGameTime.TotalSeconds); 
                    }
                    else 
                    {
                        position_player.Y = (int)(position_player.Y / pixels_per_step)*pixels_per_step;
                        ps = PLAYER_STATE.IDLE;
                    }
                }
                else 
                {
                    position_player.Y = (int)(position_player.Y - pixels_per_sec*(float)gameTime.ElapsedGameTime.TotalSeconds);                  
                }
                break;

            case PLAYER_STATE.MOVING_DOWN:
                texture_player = texture_player_moving;
                if(Keyboard.GetState().IsKeyUp(Keys.Down))
                {
                    if((int)(position_player.Y + pixels_per_sec*(float)gameTime.ElapsedGameTime.TotalSeconds) < (int)(position_player.Y / pixels_per_step + 1)*pixels_per_step)
                    {
                        position_player.Y = (int)(position_player.Y + pixels_per_sec*(float)gameTime.ElapsedGameTime.TotalSeconds); 
                    }
                    else 
                    {
                        position_player.Y = (int)(position_player.Y / pixels_per_step + 1)*pixels_per_step;
                        ps = PLAYER_STATE.IDLE;
                    }
                }
                else 
                {
                    position_player.Y = (int)(position_player.Y + pixels_per_sec*(float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                break;

            case PLAYER_STATE.MOVING_LEFT:
                texture_player = texture_player_moving;
                if(Keyboard.GetState().IsKeyUp(Keys.Left))
                {
                    if((int)(position_player.X - pixels_per_sec*(float)gameTime.ElapsedGameTime.TotalSeconds) > (int)(position_player.X / pixels_per_step)*pixels_per_step)
                    {
                        position_player.X = (int)(position_player.X - pixels_per_sec*(float)gameTime.ElapsedGameTime.TotalSeconds); 
                    }
                    else 
                    {
                        position_player.X = (int)(position_player.X / pixels_per_step)*pixels_per_step;
                        ps = PLAYER_STATE.IDLE;
                    }
                }
                else
                {
                    position_player.X = (int)(position_player.X - pixels_per_sec*(float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                break;

            case PLAYER_STATE.MOVING_RIGHT:
                texture_player = texture_player_moving;
                if(Keyboard.GetState().IsKeyUp(Keys.Right))
                {
                    if((int)(position_player.X + pixels_per_sec*(float)gameTime.ElapsedGameTime.TotalSeconds) < (int)(position_player.X / pixels_per_step + 1)*pixels_per_step)
                    {
                        position_player.X = (int)(position_player.X + pixels_per_sec*(float)gameTime.ElapsedGameTime.TotalSeconds); 
                    }
                    else 
                    {
                        position_player.X = (int)(position_player.X / pixels_per_step + 1)*pixels_per_step;
                        ps = PLAYER_STATE.IDLE;
                    }
                }
                else
                {
                    position_player.X = (int)(position_player.X + pixels_per_sec*(float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                break;
        }
    }

    void IScene.Draw(GameTime gameTime, SpriteBatch spriteBatch, int scale_factor)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
        spriteBatch.Draw(texture_player, new Rectangle((int)position_player.X, (int)position_player.Y, texture_player.Width*scale_factor, texture_player.Height*scale_factor),Color.White);
        spriteBatch.End();
    }
}