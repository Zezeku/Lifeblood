using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lifeblood;

public interface IScene
{
    void LoadContent(ContentManager content);

    void Update(GameTime gameTime);       

    void Draw(GameTime gameTime, SpriteBatch spriteBatch, int scale_factor);
}