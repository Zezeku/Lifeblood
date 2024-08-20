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

    Texture2D texture_overworld;
    //Color[] retrievedTexData;

    Texture2D texture_player_idle;
    Texture2D texture_player_moving;
    Texture2D texture_player;
    Vector2 position_player;
    int sprite_frame;
    int sprite_direction;
    float sprite_fps_target;
    float sprite_fps;

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
        texture_overworld = content.Load<Texture2D>("worldMap");
        texture_player_idle = content.Load<Texture2D>("AvatarSpriteSheet");
        texture_player_moving = content.Load<Texture2D>("AvatarSpriteSheet");
        texture_player = texture_player_idle;
        //sprite_frame = 0;
        //sprite_direction = 0;
        //sprite_fps_target = 12;
        //sprite_fps = 0;


        // NOTE: Using monongame to get texture's pixel data
        // retrievedTexData = new Color[256*224*4];
        // texture_overworld.GetData(0, new Rectangle(0,0,256,224), retrievedTexData, 0, 256*224);
        // Debug.WriteLine("rtd: " + retrievedTexData[255].ToString());

        position_player = new Vector2(8*3*10,8*3*10); // must be divisible by 8
        pixels_per_step = (int)16; //)(16*(float)0.5*3); //need scale factor here. 16 is width of sprite we want. Not using texture.Width because its a spritesheet
        sec_per_step = (float)1/16;
        pixels_per_sec = (int)(pixels_per_step / sec_per_step);

        ps = PLAYER_STATE.IDLE;
    }

    void IScene.Update(GameTime gameTime)
    {       

        switch(ps)
        {
            case PLAYER_STATE.IDLE:
                texture_player = texture_player_idle;
                //sprite_frame = 6;
                if(Keyboard.GetState().IsKeyDown(Keys.Up))
                {    
                    ps = PLAYER_STATE.MOVING_UP;
                    //sprite_direction = 2;
                }
                if(Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    ps = PLAYER_STATE.MOVING_DOWN;
                    //sprite_direction = 6;
                }
                if(Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    ps = PLAYER_STATE.MOVING_LEFT;
                    //sprite_direction = 4;
                }
                if(Keyboard.GetState().IsKeyDown(Keys.Right))
                {    
                    ps = PLAYER_STATE.MOVING_RIGHT;
                    //sprite_direction = 0;
                }
                break;

            case PLAYER_STATE.MOVING_UP:
                texture_player = texture_player_moving;
                /*if(sprite_fps > 1 / sprite_fps_target)
                {
                    sprite_frame = (sprite_frame + 1 ) % 8;
                    sprite_fps = 0;
                }*/
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
                    //sprite_fps += (float)gameTime.ElapsedGameTime.TotalSeconds;                  
                }
                break;

            case PLAYER_STATE.MOVING_DOWN:
                texture_player = texture_player_moving;
                /*if(sprite_fps > 1 / sprite_fps_target)
                {
                    sprite_frame = (sprite_frame + 1 ) % 8;
                    sprite_fps = 0;
                }*/
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
                    //sprite_fps += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                break;

            case PLAYER_STATE.MOVING_LEFT:
                texture_player = texture_player_moving;
               /*if(sprite_fps > 1 / sprite_fps_target)
                {
                    sprite_frame = (sprite_frame + 1 ) % 8;
                    sprite_fps = 0;
                }*/
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
                    //sprite_fps += (float)gameTime.ElapsedGameTime.TotalSeconds;                  
                }
                break;

            case PLAYER_STATE.MOVING_RIGHT:
                texture_player = texture_player_moving;
               /*if(sprite_fps > 1 / sprite_fps_target)
                {
                    sprite_frame = (sprite_frame + 1 ) % 8;
                    sprite_fps = 0;
                }*/
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
                    //sprite_fps += (float)gameTime.ElapsedGameTime.TotalSeconds;                  
                }
                break;
        }
    }

    void IScene.Draw(GameTime gameTime, SpriteBatch spriteBatch, int scale_factor)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
        spriteBatch.Draw(texture_overworld, new Rectangle(0,0, 256*scale_factor*2, 224*scale_factor*2), Color.White);
        spriteBatch.Draw(texture_player, new Rectangle((int)position_player.X, (int)position_player.Y, 16*scale_factor, 16*scale_factor), new Rectangle(0,0,16,16), Color.White);
        spriteBatch.End();
    }
}