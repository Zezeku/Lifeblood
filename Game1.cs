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

    IScene scene;
    Stack<IScene> stack_scene;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 256*scale_factor;
        _graphics.PreferredBackBufferHeight = 244*scale_factor;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // UNCAPS FPS. UPDATES ARE NOT ADJUSTED TO ACCOUNT FOR THIS. TESTING SEVERE CASES ONLY
        // _graphics.SynchronizeWithVerticalRetrace = false;
        // this.IsFixedTimeStep = false;
    }

    protected override void Initialize()
    {
        GameTime gt = new GameTime();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("alagard");


        stack_scene = new Stack<IScene>();
        stack_scene.Push(new Zone_Start());
        stack_scene.Push(new TitleScene());
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

        _spriteBatch.Begin();
        _spriteBatch.DrawString(_font, "FPS: " + fps.ToString("0.00"), new Vector2(20,20), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

}
