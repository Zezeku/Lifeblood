using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lifeblood;

public class Zone_Start: IScene
{
    Boolean isPlaying;

    Texture2D texture_player;
    Vector2 position_player;
    Vector2 velocity;
    int speed;


    bool isUpPresesd;
    bool isDownPresesd;
    bool isLeftPressed;
    bool isRightPressed;

    bool IScene.IsPlaying()
    {
        return isPlaying;
    }

    void IScene.LoadContent(ContentManager content)
    {
        isPlaying = true;
        texture_player = content.Load<Texture2D>("AvatarSprite");
        position_player = new Vector2(256/2,244/2);
        velocity = new Vector2(0,0);
        speed = texture_player.Width/8*3;

        isUpPresesd = false;
        isDownPresesd = false;
        isLeftPressed = false;
        isRightPressed = false;
    }

    void IScene.Update(GameTime gameTime)
    {
        if(!isRightPressed && Keyboard.GetState().IsKeyDown(Keys.Right) && velocity == Vector2.Zero)
        {
            //isRightPressed = true;
            velocity.X = 1;
        }
        if(Keyboard.GetState().IsKeyUp(Keys.Right))
        {
            isRightPressed = false;
        }

        if(!isLeftPressed && Keyboard.GetState().IsKeyDown(Keys.Left) && velocity == Vector2.Zero)
        {
            //isLeftPressed = true;
            velocity.X = -1;
        }
        if(Keyboard.GetState().IsKeyUp(Keys.Left))
        {
            isLeftPressed = false;
        }

        if(!isUpPresesd && Keyboard.GetState().IsKeyDown(Keys.Up) && velocity == Vector2.Zero)
        {
            //isUpPresesd = true;
            velocity.Y = -1;
        }
        if(Keyboard.GetState().IsKeyUp(Keys.Up))
        {
            isUpPresesd = false;
        }

        if(!isDownPresesd && Keyboard.GetState().IsKeyDown(Keys.Down) && velocity == Vector2.Zero)
        {
            //isDownPresesd = true;
            velocity.Y = 1;
        } 
        if(Keyboard.GetState().IsKeyUp(Keys.Down))
        {
            isDownPresesd = false;
        }

        position_player += velocity * speed;
        velocity = Vector2.Zero;
    }

    void IScene.Draw(GameTime gameTime, SpriteBatch spriteBatch, int scale_factor)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
        spriteBatch.Draw(texture_player, new Rectangle((int)position_player.X, (int)position_player.Y, texture_player.Width*scale_factor, texture_player.Height*scale_factor),Color.White);
        spriteBatch.End();
    }
}