using SFML.System;
using SFML.Window;
using SFML.Graphics;

using Latte;


using Window = Latte.Window;


namespace LatteArt;


public readonly struct BoundLimitCollisions
{
    public required bool AtLeft { get; init; }
    public required bool AtRight { get; init; }
    public required bool AtTop { get; init; }
    public required bool AtBottom { get; init; }
    
    
    public bool HasAnyCollision()
        => AtLeft || AtRight || AtTop || AtBottom;
    
    public bool LeftRightCollision() => AtLeft && AtRight;
    public bool TopBottomCollision() => AtTop && AtBottom;
}


public class CanvasView
{
    public Window Window { get; private set; }
    
    // how much the mouse position changed since the last frame update
    public Vector2f WorldMousePositionDelta => _oldWorldMousePosition - Window.WorldMousePosition;
    private Vector2f _oldWorldMousePosition;

    public float MouseScrollDelta { get; set; }

    public FloatRect Bounds => Window.View.ViewToRect();
    public FloatRect? BoundsLimit { get; set; }
    
    public float ZoomOutFactor { get; private set; } = 1.15f; 
    public float ZoomInFactor { get; private set; } = 0.85f;
    
    public float MaxZoomOut { get; private set; } = 2300f;
    public float MaxZoomIn { get; private set; } = 500f;
    
    public bool IsAtMaxZoom => Window.View.Size.X >= MaxZoomOut;
    public bool IsAtMinZoom => Window.View.Size.Y <= MaxZoomIn;
    
    public bool IsGrabbingView => Window.HasFocus() && Mouse.IsButtonPressed(Mouse.Button.Right);
    
    
    public CanvasView(Window window)
    {
        Window = window;
        
        Window.MouseWheelScrolled += (_, args) => MouseScrollDelta = args.Delta;
    }
    

    public void Update()
    {
        if (IsGrabbingView)
            ProcessViewGrabMovement();
        
        if (MouseScrollDelta != 0f && (MouseScrollDelta > 0 ? !IsAtMinZoom : !IsAtMaxZoom))
        {
            float zoom = MouseScrollDelta > 0 ? ZoomInFactor : ZoomOutFactor;
            Window.View.Zoom(zoom);
        }

        CheckBoundsLimits();
        
        MouseScrollDelta = 0f;
        
        _oldWorldMousePosition = Window.WorldMousePosition;
    }

    private void ProcessViewGrabMovement()
    {
        Vector2f delta = WorldMousePositionDelta;
        
        if (BoundsLimit is not null)
        {
            BoundLimitCollisions collisions = GetBoundLimitCollisions();

            if (collisions.LeftRightCollision())
                delta.X = 0;

            if (collisions.TopBottomCollision())
                delta.Y = 0;
        }
            
        Window.View.Move(delta);
    }
    
    private void CheckBoundsLimits()
    {
        if (BoundsLimit is null)
            return;
        
        BoundLimitCollisions collisions = GetBoundLimitCollisions();

        bool horizontalLocked = false;
        bool verticalLocked = false;

        if (collisions.LeftRightCollision())
        {
            Window.View.Center = Window.View.Center with { X = BoundsLimit.Value.Position.X + BoundsLimit.Value.Size.X / 2f };
            horizontalLocked = true;
        }

        if (collisions.TopBottomCollision())
        {
            Window.View.Center = Window.View.Center with { Y = BoundsLimit.Value.Position.Y + BoundsLimit.Value.Size.Y / 2f };
            verticalLocked = true;
        }
        
        if (collisions.AtLeft && !horizontalLocked)
            Window.View.MoveToPosition(new(BoundsLimit.Value.Position.X, Bounds.Position.Y));
        
        else if (collisions.AtRight && !horizontalLocked)
            Window.View.MoveToPosition(new(BoundsLimit.Value.Position.X + BoundsLimit.Value.Width - Bounds.Width, Bounds.Position.Y));
        
        if (collisions.AtTop && !verticalLocked)
            Window.View.MoveToPosition(new(Bounds.Position.X, BoundsLimit.Value.Position.Y));
        
        else if (collisions.AtBottom && !verticalLocked)
            Window.View.MoveToPosition(new(Bounds.Position.X, BoundsLimit.Value.Position.Y + BoundsLimit.Value.Height - Bounds.Height));
        
    }


    private BoundLimitCollisions GetBoundLimitCollisions()
    {
        if (BoundsLimit is null)
            throw new NullReferenceException("Bounds limit is not defined.");
        
        return new BoundLimitCollisions
        {
            AtTop = Bounds.Position.Y < BoundsLimit.Value.Position.Y,
            AtLeft = Bounds.Position.X < BoundsLimit.Value.Position.X,
            AtRight = Bounds.Position.X + Bounds.Width > BoundsLimit.Value.Position.X + BoundsLimit.Value.Width,
            AtBottom = Bounds.Position.Y + Bounds.Height > BoundsLimit.Value.Position.Y + BoundsLimit.Value.Height
        };
    }
}