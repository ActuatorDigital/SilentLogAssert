# Silent Log Assert

Prevent expected exceptions from appearing in the Unity Console during TestRunner execution; as anything showing up in red in the console after a test run can be quite misleading.

## Features

- No more red icons in the console after running tests that require error logs.
  - Replaces the unity debug logger for the life time of the object. This allows you to write unit tests that cause logs, errors, or warnings without actually have them show up there during the test.
- Assert.Throws style call

## Basic Usage

Where you previously may have used the LogAssert provided by Unity, a la.

```csharp
// Act
mockParentView.Draw();

// Assert
LogAssert.Expect(LogType.Error, expectedLog);
```

This becomes

```csharp
// Act
void Act() => mockParentView.Draw();

// Assert
SilentLogAssert.Expect(LogType.Error, expectedLog, Act);
```

## Installation

Add to unity via the package manager. Use 'Add git package' with `https://github.com/AnImaginedReality/SilentLogAssert.git`.
