<script>
  import Cinema from "./lib/Cinema.svelte";
  import Films from "./lib/Films.svelte";
  import Genres from "./lib/Genres.svelte";

  let selectedComponent = null
  let components = [
    {
      instance: Cinema,
      name: "Сеансы"
    },
    {
      instance: Genres,
      name: "Жанры"
    },
    {
      instance: Films,
      name: "Фильмы"
    }
  ]

</script>

<nav class="container">
  <ul>
    <li><strong>Inueco</strong></li>
  </ul>

  <ul>
    {#each components as component}
      {#if selectedComponent == component.instance}
        <strong><li><button style="all:unset; cursor: pointer" on:click={() => selectedComponent = component.instance}>{component.name}</button></li></strong>
      {:else}
        <li><button style="all:unset; cursor: pointer" on:click={() => selectedComponent = component.instance}>{component.name}</button></li>
      {/if}

    {/each}
  </ul>

</nav>

<div class="container">
  <svelte:component this={selectedComponent}></svelte:component>
</div>
