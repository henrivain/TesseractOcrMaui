namespace TesseractOcrMaui.Iterables;

/// <summary>
/// Base type for <see cref="IDisposable"/> object that is dependant on 
/// another disposable object. Automatically disposes object instance if 
/// parent is disposed.
/// </summary>
public abstract class ParentDependantDisposableObject : DisposableObject 
{
    /// <summary>
    /// New base type for <see cref="IDisposable"/> object that is dependant on 
    /// another disposable object. Automatically disposes object instance if 
    /// parent is disposed.
    /// </summary>
    /// <param name="dependencyObject">Object that's disposal is tracked.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="dependencyObject"/> is <see langword="null"/>.</exception>
    internal ParentDependantDisposableObject(DisposableObject dependencyObject)
    {
        ArgumentNullException.ThrowIfNull(dependencyObject);

        _dependencyObject = dependencyObject;
        _dependencyObject.Disposed += (_, _) =>
        {
            DidParentDispose = true;
            Dispose();
        };
    }

    /// <summary>
    /// <see langword="true"/> if parent object was disposed and caused 
    /// this object instance to also get disposed, otherwise <see langword="false"/>
    /// </summary>
    protected bool DidParentDispose { get; private set; } = false;

    private protected readonly DisposableObject _dependencyObject;

    internal DisposableObject GetDependencyObject() => _dependencyObject;
}
