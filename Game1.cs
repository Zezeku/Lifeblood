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
    Texture2D _texture_splash;
    Texture2D _texture_title;
    Texture2D _texture_pointer;
    Texture2D _texture_current;
    Texture2D _texture_next;

    Color _color_fade;
    SpriteFont _font;
    int scale_titleFont = 4;
    Boolean isTitleDraw;

    double delayTimer = 0;
    double delayTarget = 1.5;


    int pointerPosition;
    int pointerOffset = 0;
    Boolean isDownPressed = false;
    Boolean isUpPressed = false;
    Boolean isEnterPressed = false;
    Boolean isGameStart = false;

    enum BasicState
    {
        Splash,
        Title,
        Game,
        ScreenFadeIn,
        ScreenFadeOut,
        Exit
    }
    BasicState bs;
    Stack<BasicState> state_stack = new Stack<BasicState>();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 256*scale_factor;
        _graphics.PreferredBackBufferHeight = 244*scale_factor;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        isTitleDraw = false;
        pointerPosition = 244*scale_factor/2 - 10 ;
    }

    protected override void Initialize()
    {
        GameTime gt = new GameTime();
        state_stack.Push(BasicState.Splash);
        state_stack.Push(BasicState.ScreenFadeIn);
        bs = state_stack.Pop();
        _color_fade = new Color(0, 0, 0, 255);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _texture_splash = Content.Load<Texture2D>("SplashScreen");
        _texture_title = Content.Load<Texture2D>("TitleScreen");
        _texture_pointer = Content.Load<Texture2D>("TitlePointer");
        _texture_current = _texture_splash;
        _font = Content.Load<SpriteFont>("alagard");

    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        switch (bs)
        {
            case BasicState.Splash:
                if(delayTimer < delayTarget)
                {
                    delayTimer += gameTime.ElapsedGameTime.TotalSeconds;    
                }
                else
                {
                    _texture_next = _texture_title;
                    state_stack.Push(BasicState.Title);
                    state_stack.Push(BasicState.ScreenFadeIn);
                    state_stack.Push(BasicState.ScreenFadeOut);
                    bs = state_stack.Pop();
                }

                break;
            
            case BasicState.Title:
                isTitleDraw = true;

                if(!isDownPressed && Keyboard.GetState().IsKeyDown(Keys.Down) && pointerOffset != 100)
                {
                    isDownPressed = true;
                    pointerOffset += 100;
                }

                if(Keyboard.GetState().IsKeyUp(Keys.Down))
                {
                    isDownPressed = false;
                }

                if(!isUpPressed && Keyboard.GetState().IsKeyDown(Keys.Up) && pointerOffset != 0)
                {
                    isUpPressed = true;
                    pointerOffset -= 100;
                }

                if(Keyboard.GetState().IsKeyUp(Keys.Up))
                {
                    isUpPressed = false;
                }

                if(Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    isEnterPressed = true;
                    isTitleDraw = false;
                    _texture_next = _texture_splash;
                    state_stack.Push(BasicState.Game);
                    state_stack.Push(BasicState.ScreenFadeIn);
                    state_stack.Push(BasicState.ScreenFadeOut);
                    bs = state_stack.Pop();
                }

                break;
            
            case BasicState.Game:

                break;
            
            case BasicState.ScreenFadeIn:
                if(_color_fade.R < 254)
                {
                    _color_fade.R += 2;
                    _color_fade.G += 2;
                    _color_fade.B += 2;
                }
                else 
                {
                    bs = state_stack.Pop();
                }
                break;
            
            case BasicState.ScreenFadeOut:
                if(_color_fade.R > 1)
                {
                    _color_fade.R -= 2;
                    _color_fade.G -= 2;
                    _color_fade.B -= 2;
                }
                else
                {
                    _texture_current = _texture_next;
                    bs = state_stack.Pop();
                }
                break;
            
            case BasicState.Exit:

                break;
        }

        base.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);


        _spriteBatch.Draw(_texture_current, new Rectangle(0,0,256*scale_factor,244*scale_factor), _color_fade); 

        if(isTitleDraw)
        {
            _spriteBatch.Draw(_texture_pointer, new Rectangle(256*scale_factor/3-100, pointerPosition + pointerOffset, 8*10, 8*10), _color_fade);
            _spriteBatch.DrawString(_font, "New Game", new Vector2(256*scale_factor/3, 244*scale_factor/2), _color_fade, 0, new Vector2(0,0), scale_titleFont, 0, 0);
            _spriteBatch.DrawString(_font, "Continue", new Vector2(256*scale_factor/3, 244*scale_factor/2 + 100), _color_fade, 0, new Vector2(0,0), scale_titleFont, 0, 0);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }

}
