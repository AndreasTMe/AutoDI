# AutoDI

[![Tests](https://github.com/andreastdev/AutoDI/actions/workflows/tests.yml/badge.svg)](https://github.com/andreastdev/AutoDI/actions/workflows/tests.yml)

## Attributes

- `IsDependencyAttribute`: Marks a type (class or interface) as a dependency.
- `InjectDependencyAttribute`: Connects a class implementation to an interface, abstract class or to itself.

## Analyzers

| Rule ID    | Category | Severity | Notes                                                       |
|------------|----------|----------|-------------------------------------------------------------|
| AutoDI0001 | AutoDI   | Error    | A dependency must either implement or be the injected type. |