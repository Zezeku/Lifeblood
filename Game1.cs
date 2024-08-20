using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lifeblood;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private int scale_factor = 3;
    private float fps;
    SpriteFont _font;

    Texture2D texture_sq;
    //bool isZone;

    IScene scene;
    Stack<IScene> stack_scene;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 256*scale_factor;
        _graphics.PreferredBackBufferHeight = 224*scale_factor;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // UNCAPS FPS. UPDATES ARE NOT ADJUSTED TO ACCOUNT FOR THIS. TESTING SEVERE CASES ONLY
        //_graphics.SynchronizeWithVerticalRetrace = false;
        // this.IsFixedTimeStep = false;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("alagard");
        texture_sq = Content.Load<Texture2D>("AvatarSprite");
        //isZone = false;

        stack_scene = new Stack<IScene>();
        stack_scene.Push(new Zone_Start());
        //stack_scene.Push(new TitleScene());
        scene = stack_scene.Pop();
        scene.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        fps = (float)(1.0 / gameTime.ElapsedGameTime.TotalSeconds);

        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if(scene.IsPlaying())
        {
            scene.Update(gameTime);
        }
        // can put an if else here when its time toreassign a new scene using scene manager
        else 
        {
            if(stack_scene.Count > 0)
            {
                scene = stack_scene.Pop();
                scene.LoadContent(Content);
                //isZone = true;
            }
            else 
            {
                Exit();
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        scene.Draw(gameTime, _spriteBatch, scale_factor);

        //TESTING MOVEMENT ALIGNMENT
        // if(isZone)
        // {
        //     _spriteBatch.Begin();
        //     for(int i = 0; i < _graphics.PreferredBackBufferHeight/(8*scale_factor)+1; i++)
        //     {
        //         for(int j = 0; j < _graphics.PreferredBackBufferWidth/(8*scale_factor)+1; j++)
        //         {
        //             int checker = ((i%2)+(j%2))%2; 
        //             _spriteBatch.Draw(texture_sq, new Rectangle((j*8-4)*scale_factor, (i*8-4)*scale_factor, 8*scale_factor, 8*scale_factor), new Color(255*checker, 255*checker, 255*checker, 1*(1-checker)));
        //         }
        //     }
        //     _spriteBatch.End();
        // }
        
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_font, "FPS: " + fps.ToString("0.00"), new Vector2(20,20), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

}
