<script>
  import { onMount } from "svelte";
  import Cinema from "./lib/Cinema.svelte";
  import Films from "./lib/Films.svelte";
  import Genres from "./lib/Genres.svelte";
  import Auth from "./lib/Auth.svelte";

  let User = undefined

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

{#if User == undefined}
  <div class="container">
    <Auth bind:User={User}/>
  </div>
{/if}


<nav class="container">
  <ul>
    <li>
      <strong>Inueco</strong>
      {#if User != undefined}
        ({User.username})
      {/if}
    </li>
  </ul>

  {#if User != undefined}
    <ul>
      {#each components as component}
        {#if selectedComponent == component.instance}
          <strong><li><button style="all:unset; cursor: pointer" on:click={() => selectedComponent = component.instance}>{component.name}</button></li></strong>
        {:else}
          <li><button style="all:unset; cursor: pointer" on:click={() => selectedComponent = component.instance}>{component.name}</button></li>
        {/if}
      {/each}
    </ul>
  {/if}
</nav>

<div class="container">
  <svelte:component this={selectedComponent}></svelte:component>
</div>
